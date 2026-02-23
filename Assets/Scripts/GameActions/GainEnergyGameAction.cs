using UnityEngine;

public class GainEnergyGameAction : GameAction
{
    public int Amount { get; set; }
    public GainEnergyGameAction(int amount)
    {
        Amount = amount;
    }
}
