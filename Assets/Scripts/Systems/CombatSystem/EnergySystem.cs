using System.Collections;
using UnityEngine;

public class EnergySystem : Singleton<EnergySystem>
{
    [SerializeField] private EnergyUI energyUI;

    private const int MAX_ENERGY = 3;
    private int currentEnergy = MAX_ENERGY;

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<SpendEnergyGameAction>(SpendEnergyPerformer);
        ActionSystem.AttachPerformer<RefillEnergyGameAction>(RefillEnergyPerformer);
        ActionSystem.AttachPerformer<GainEnergyGameAction>(GainEnergyPerformer);
        ActionSystem.SubscribeReaction<EnemyTurnGameAction>(EnemyTurnPostReaction, ReactionTiming.POST);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<SpendEnergyGameAction>();
        ActionSystem.DetachPerformer<RefillEnergyGameAction>();
        ActionSystem.DetachPerformer<GainEnergyGameAction>();
        ActionSystem.UnsubscribeReaction<EnemyTurnGameAction>(EnemyTurnPostReaction, ReactionTiming.POST);
    }

    public bool HasEnoughEnergy(int energy)
    {
        return currentEnergy >= energy;
    }

    private IEnumerator SpendEnergyPerformer(SpendEnergyGameAction spendEnergyGA)
    {
        currentEnergy -= spendEnergyGA.Amount;
        energyUI.UpdateEnergyText(currentEnergy);
        yield return null;
    }

    private IEnumerator GainEnergyPerformer(GainEnergyGameAction gainEnergyGA)
    {
        currentEnergy = currentEnergy + gainEnergyGA.Amount;
        energyUI.UpdateEnergyText(currentEnergy);
        yield return null;
    }
    private IEnumerator RefillEnergyPerformer(RefillEnergyGameAction refillEnergyGA)
    {
        currentEnergy = MAX_ENERGY;
        energyUI.UpdateEnergyText(currentEnergy);
        yield return null;
    }

    private void EnemyTurnPostReaction(EnemyTurnGameAction enemyTurnGA)
    {
        RefillEnergyGameAction refillEnergyGA = new();
        ActionSystem.Instance.AddReaction(refillEnergyGA);
    }
}
