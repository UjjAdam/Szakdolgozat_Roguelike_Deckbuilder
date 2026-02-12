using System.Collections;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    [SerializeField] private GameObject enemyDamagedVFX;
    [SerializeField] private GameObject heroDamagedVFX;

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<DealDamageGameAction>(DealDamagePerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<DealDamageGameAction>();
    }

    private IEnumerator DealDamagePerformer(DealDamageGameAction dealDamageGA)
    {
        foreach (var target in dealDamageGA.Targets)
        {
            // (safe-guard for multi-hit effects)
            if (target == null)
                continue;

            target.Damage(dealDamageGA.Amount);

            if (target is EnemyView enemyView)
            {
                    Instantiate(enemyDamagedVFX, target.transform.position, Quaternion.identity);
            }
            else
            {
                    Instantiate(heroDamagedVFX, target.transform.position, Quaternion.identity);
            }

            // kis várakozás
            yield return new WaitForSeconds(0.15f);

            // Csekkolás hogy a célpont még nem tûnt el azaz nem halt meg közben
            if (target != null && target.CurrentHealth <= 0)
            {
                if (target is EnemyView enemyView1)
                {
                    KillEnemyGameAction killEnemyGA = new(enemyView1);
                    ActionSystem.Instance.AddReaction(killEnemyGA);
                }
                else if (target is HeroView heroView)
                {
                    //Hero dies
                    KillHeroGameAction killHeroGA = new(heroView);
                    ActionSystem.Instance.AddReaction(killHeroGA);

                    //Game Over Scene, Hero lost all health
                    LoseGameAction loseGA = new LoseGameAction();
                    ActionSystem.Instance.AddReaction(loseGA);

                    Time.timeScale = 0f;
                }
            }
        }
    }
}
