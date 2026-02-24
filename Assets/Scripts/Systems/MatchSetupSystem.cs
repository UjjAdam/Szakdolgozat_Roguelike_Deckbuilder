using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchSetupSystem : MonoBehaviour
{
    [SerializeField] private HeroData heroData;
    [SerializeField] private List<EnemyData> enemyDatas;

    // Start is a coroutine so we can wait for session systems to be ready
    private IEnumerator Start()
    {

        // Wait until the Session-scene HeroSystem has been created/initialized
        yield return new WaitUntil(() => HeroSystem.Instance != null);

        CardSystem.Instance.ResetDeck();

        HeroSystem.Instance.Setup(heroData);
        CardSystem.Instance.Setup(heroData.StarterDeck);

        GenerateEnemies();

        // PerkSystem holds the session-acquired perks
        if (PerkSystem.Instance != null)
            PerkSystem.Instance.ReapplyPerks();

        DrawCardsGameAction drawCardsGA = new(5);
        ActionSystem.Instance.Perform(drawCardsGA);
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
