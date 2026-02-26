using System;
using UnityEngine;

public class OnGainEnergyCondition : PerkCondition
{
    public override bool SubConditionIsMet(GameAction gameAction)
    {
        //future: check if attacker is above x health ...
        return true;
    }

    public override void SubscribeCondition(Action<GameAction> reaction)
    {
        ActionSystem.SubscribeReaction<GainEnergyGameAction>(reaction, reactionTiming);
    }

    public override void UnsubscribeCondition(Action<GameAction> reaction)
    {
        ActionSystem.UnsubscribeReaction<GainEnergyGameAction>(reaction, reactionTiming);
    }
}
