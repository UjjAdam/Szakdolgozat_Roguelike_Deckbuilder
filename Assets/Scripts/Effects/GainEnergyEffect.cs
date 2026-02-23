using System.Collections.Generic;
using UnityEngine;

public class GainEnergyEffect : Effect
{
    [SerializeField] private int energyAmount;
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        GainEnergyGameAction gainEnergyGA = new(energyAmount);
        return gainEnergyGA;
    } 
}
