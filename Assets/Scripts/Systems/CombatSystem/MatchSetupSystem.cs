using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchSetupSystem : MonoBehaviour
{
    [SerializeField] private HeroData heroData;
    [SerializeField] private List<EnemyData> enemyDatas;

    // Starter card
    [Header("Starter Deck")]
    [SerializeField] private CardData armorUpCard;
    [SerializeField] private CardData drillStrikeCard;
    [SerializeField] private CardData overdriveCard;
    [SerializeField] private CardData mineCollapseCard;

    // Start is a coroutine so we can wait for session systems to be ready
    private IEnumerator Start()
    {
        // Wait until the Session-scene HeroSystem has been created/initialized
        yield return new WaitUntil(() => HeroSystem.Instance != null);

        var progress = ProgressSystem.Instance;

        // Only perform starter-deck reset on the very first combat of a new run.
        if (progress == null || !progress.IsRunInitialized)
        {
            CardSystem.Instance.ResetDeck();

            // Build a fresh starter deck for this run and give it to CardSystem.
            var starterDeck = BuildStarterDeck();

            // Append any cards acquired previously (should be empty at absolute new run start,
            // but this supports pre-run acquired list if any).
            if (progress != null)
            {
                var acquired = progress.GetAcquiredCards();
                if (acquired != null && acquired.Count > 0)
                {
                    starterDeck.AddRange(acquired);
                }
            }

            CardSystem.Instance.Setup(starterDeck);

            // mark run initialized so later combat loads do not overwrite the runtime deck
            progress?.MarkRunInitialized();
        }
        else
        {
            // If CardSystem was destroyed/recreated with scene, it may have an empty runtime deck.
            // Rebuild deck here only when the new CardSystem has no cards (prevents duplicates).
            if (CardSystem.Instance != null)
            {
                if (!CardSystem.Instance.HasAnyCards())
                {
                    var starterDeck = BuildStarterDeck();
                    if (progress != null)
                    {
                        var acquired = progress.GetAcquiredCards();
                        if (acquired != null && acquired.Count > 0)
                            starterDeck.AddRange(acquired);
                    }
                    CardSystem.Instance.Setup(starterDeck);
                }

                // Recreate CardView visuals for runtime hand (scene reload destroyed previous GameObjects).
                yield return CardSystem.Instance.RecreateHandViews();
            }
        }

        // always setup hero view / saved HP
        HeroSystem.Instance.Setup(heroData);

        GenerateEnemies();

        // PerkSystem holds the session-acquired perks
        if (PerkSystem.Instance != null)
            PerkSystem.Instance.ReapplyPerks();

        DrawCardsGameAction drawCardsGA = new(5);
        ActionSystem.Instance.Perform(drawCardsGA);
    }

    private List<CardData> BuildStarterDeck()
    {
        var deck = new List<CardData>();

        // Defensive fallback: if any card references are missing, fall back to heroData.StarterDeck if available.
        if (armorUpCard == null || drillStrikeCard == null || overdriveCard == null || mineCollapseCard == null)
        {
            if (heroData != null && heroData.StarterDeck != null && heroData.StarterDeck.Count > 0)
            {
                Debug.LogWarning("MatchSetupSystem: Some starter card references are not assigned — falling back to HeroData.StarterDeck.");
                return new List<CardData>(heroData.StarterDeck);
            }
            Debug.LogError("MatchSetupSystem: Starter card references are not fully assigned and HeroData.StarterDeck is empty. Returning empty deck.");
            return deck;
        }

        // 3 Armor Up
        for (int i = 0; i < 3; i++)
            deck.Add(armorUpCard);

        // 3 Drill Strike
        for (int i = 0; i < 3; i++)
            deck.Add(drillStrikeCard);

        // 1 Overdrive
        deck.Add(overdriveCard);

        // 1 Mine Collapse
        deck.Add(mineCollapseCard);

        return deck;
    }

    private void GenerateEnemies()
    {
        // random enemy az enemyDatas-ból
        List<EnemyData> enemiesToSpawn = new List<EnemyData>();
        if (enemyDatas != null && enemyDatas.Count > 0)
        {
            int count = Random.Range(1, 4);
            for (int i = 0; i < count; i++)
            {
                EnemyData chosen = enemyDatas[Random.Range(0, enemyDatas.Count)];
                enemiesToSpawn.Add(chosen);
            }
        }

        EnemySystem.Instance.Setup(enemiesToSpawn);
    }
}
