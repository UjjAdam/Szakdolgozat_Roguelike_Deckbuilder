using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : Singleton<EnemySystem>
{
    [SerializeField] private EnemyBoardView enemyBoardView;
    public List<EnemyView> Enemies => enemyBoardView.EnemyViews;

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<EnemyTurnGameAction>(EnemyTurnPerformer);
        ActionSystem.AttachPerformer<AttackHeroGameAction>(AttackHeroPerformer);
        ActionSystem.AttachPerformer<KillEnemyGameAction>(KillEnemyPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<EnemyTurnGameAction>();
        ActionSystem.DetachPerformer<AttackHeroGameAction>();
        ActionSystem.DetachPerformer<KillEnemyGameAction>();

    }

    public void Setup(List<EnemyData> enemyDatas)
    {
        foreach (var enemyData in enemyDatas)
        {
            enemyBoardView.AddEnemy(enemyData);
        }
    }


    //Performer

    private IEnumerator EnemyTurnPerformer(EnemyTurnGameAction enemyTurnGA)
    {
        foreach (var enemy in enemyBoardView.EnemyViews)
        {
            int burnStacks = enemy.GetStatusEffectStacks(StatusEffectType.BURN);
            if (burnStacks > 0)
            {
                ApplyBurnGameAction applyBurnGA = new(burnStacks, enemy);
                ActionSystem.Instance.AddReaction(applyBurnGA);
            }

            // enemy data alapján dönt:
            // -AttackPower > 0 -> attack hero
            // -ShieldPower > 0 -> ARMOR az összes élõ ellenségre
            // -BurnPower > 0 -> BURN hero
            if (enemy.AttackPower > 0)
            {
                AttackHeroGameAction attackHeroGA = new(enemy);
                ActionSystem.Instance.AddReaction(attackHeroGA);
            }
            else if (enemy.ShieldPower > 0)
            {
                // az összes élõ ellenséget targeteli
                List<CombatantView> aliveEnemies = new();
                foreach (var e in enemyBoardView.EnemyViews)
                {
                    if (e.CurrentHealth > 0)
                        aliveEnemies.Add(e);
                }
                if (aliveEnemies.Count > 0)
                {
                    AddStatusEffectGameAction addArmorGA = new(StatusEffectType.ARMOR, enemy.ShieldPower, aliveEnemies);
                    ActionSystem.Instance.AddReaction(addArmorGA);
                }
            }
            else if (enemy.BurnPower > 0)
            {
                // burn stacket rak a hõsre
                List<CombatantView> heroTarget = new() { HeroSystem.Instance.HeroView };
                AddStatusEffectGameAction addBurnGA = new(StatusEffectType.BURN, enemy.BurnPower, heroTarget);
                ActionSystem.Instance.AddReaction(addBurnGA);
            }
        }
        yield return null;
        
    }

    private IEnumerator AttackHeroPerformer(AttackHeroGameAction attackHeroGA)
    {
        EnemyView attacker = attackHeroGA.Attacker;

        //Attack "animation"
        Tween tween = attacker.transform.DOMoveY(attacker.transform.position.y + 1f, 0.15f);
        yield return tween.WaitForCompletion();
        attacker.transform.DOMoveY(attacker.transform.position.y - 1f, 0.25f);

        //Deal Damage
        DealDamageGameAction dealDamageGA = new(attacker.AttackPower, new() {HeroSystem.Instance.HeroView }, attackHeroGA.Caster);
        ActionSystem.Instance.AddReaction(dealDamageGA);
    }

    private IEnumerator KillEnemyPerformer(KillEnemyGameAction killEnemyGA)
    {
        yield return enemyBoardView.RemoveEnemy(killEnemyGA.EnemyView);

        // Check for victory
        if (enemyBoardView.EnemyViews.Count == 0)
        {
            WinGameAction winGA = new WinGameAction();
            ActionSystem.Instance.AddReaction(winGA);
        }

    }

    public void Reset()
    {
        if (enemyBoardView != null && enemyBoardView.EnemyViews != null)
        {
            foreach (var enemy in new List<EnemyView>(enemyBoardView.EnemyViews))
            {
                Destroy(enemy.gameObject);
            }
            enemyBoardView.EnemyViews.Clear();
        }
        DOTween.KillAll(); 
    }
}
