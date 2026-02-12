using System.Collections;
using UnityEngine;

public class BurnSystem : MonoBehaviour
{
    [SerializeField] private GameObject burnVFX;

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<ApplyBurnGameAction>(ApplyBurnPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<ApplyBurnGameAction>();
    }

    private IEnumerator ApplyBurnPerformer(ApplyBurnGameAction applyBurnGA)
    {
        CombatantView target = applyBurnGA.Target;
        Instantiate(burnVFX, target.transform.position, Quaternion.identity);

        target.Damage(applyBurnGA.BurnDamage);
        target.RemoveStatusEffect(StatusEffectType.BURN, 1);

        // Burn általi halál
        if (target.CurrentHealth <= 0)
        {
            if (target is EnemyView enemyView)
            {
                KillEnemyGameAction killEnemyGA = new(enemyView);
                ActionSystem.Instance.AddReaction(killEnemyGA);
            }
            else if (target is HeroView heroView)
            {
                // Hero died
                KillHeroGameAction killHeroGA = new(heroView);
                ActionSystem.Instance.AddReaction(killHeroGA);

                // Game over
                LoseGameAction loseGA = new LoseGameAction();
                ActionSystem.Instance.AddReaction(loseGA);

                Time.timeScale = 0f;
            }
        }

        yield return new WaitForSeconds(1f);
    }
}
