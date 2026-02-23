using System.Collections;
using UnityEngine;

public class HeroSystem : Singleton<HeroSystem>
{
    // no longer serialized — register the scene HeroView at runtime
    public HeroView HeroView { get; private set; }

    // Session-level saved value. Session scene holds this system.
    private int savedCurrentHealth = -1;

    public void OnEnable()
    {
        ActionSystem.SubscribeReaction<EnemyTurnGameAction>(EnemyTurnPreReaction, ReactionTiming.PRE);
        ActionSystem.SubscribeReaction<EnemyTurnGameAction>(EnemyTurnPostReaction, ReactionTiming.POST);
        ActionSystem.AttachPerformer<KillHeroGameAction>(KillHeroGameActionPerformer);
    }

    public void OnDisable()
    {
        ActionSystem.UnsubscribeReaction<EnemyTurnGameAction>(EnemyTurnPreReaction, ReactionTiming.PRE);
        ActionSystem.UnsubscribeReaction<EnemyTurnGameAction>(EnemyTurnPostReaction, ReactionTiming.POST);
        ActionSystem.DetachPerformer<KillHeroGameAction>();
    }

    // Called by match setup when entering Combat scene
    public void Setup(HeroData heroData)
    {
        // If HeroView wasn't registered, try to find the scene instance now.
        if (HeroView == null)
        {
            HeroView = FindFirstObjectByType<HeroView>();
            if (HeroView == null)
            {
                Debug.LogWarning("HeroSystem.Setup: no HeroView found in scene. MatchSetupSystem should ensure a HeroView prefab exists in the Combat scene.");
                return;
            }
        }

        // Initialize the view with data (this sets MaxHealth and CurrentHealth to heroData.Health)
        HeroView.Setup(heroData);

        // Restore saved HP if present (Session stored it)
        if (savedCurrentHealth >= 0)
            HeroView.SetCurrentHealth(savedCurrentHealth);
        else
            savedCurrentHealth = HeroView.CurrentHealth;
    }

    // Called by MatchSetupSystem when entering Combat scene to explicitly register the scene view
    public void RegisterHeroView(HeroView heroView)
    {
        HeroView = heroView;
        if (HeroView == null) return;

        // Only apply savedCurrentHealth if we already have a valid saved value.
        if (savedCurrentHealth >= 0)
        {
            HeroView.SetCurrentHealth(savedCurrentHealth);
        }
    }

    // Called when the scene HeroView is destroyed to avoid stale reference
    public void UnregisterHeroView(HeroView heroView)
    {
        if (HeroView == heroView)
            HeroView = null;
    }

    // Called from CombatantView when hero HP changes
    public void SaveHeroHealth(int currentHealth)
    {
        savedCurrentHealth = currentHealth;
    }

    // Optional helper
    public int? GetSavedHeroHealth() => savedCurrentHealth >= 0 ? savedCurrentHealth : (int?)null;

    public void ClearSavedHeroHealth() => savedCurrentHealth = -1;



    //Reactions

    private void EnemyTurnPreReaction(EnemyTurnGameAction enemyturnGA)
    {
        DiscardAllCardsGameAction discardAllCardsGA = new();
        ActionSystem.Instance.AddReaction(discardAllCardsGA);
    }

    private void EnemyTurnPostReaction(EnemyTurnGameAction enemyturnGA)
    {
        int burnStacks = HeroView.GetStatusEffectStacks(StatusEffectType.BURN);
        if (burnStacks > 0)
        {
            ApplyBurnGameAction applyBurnGA = new(burnStacks, HeroView);
            ActionSystem.Instance.AddReaction(applyBurnGA);
        }
        DrawCardsGameAction drawCardsGA = new(5);
        ActionSystem.Instance.AddReaction(drawCardsGA);
    }

    //Performers

    private IEnumerator KillHeroGameActionPerformer(KillHeroGameAction killHeroGA)
    {
        killHeroGA.HeroView.SetRuinedSprite();
        yield break;
    }
}
