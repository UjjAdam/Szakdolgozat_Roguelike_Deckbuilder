using System;
using UnityEngine;

public class OnApplyBurnCondition : PerkCondition
{
    public override bool SubConditionIsMet(GameAction gameAction)
    {
        if (gameAction is ApplyBurnGameAction burnGA)
        {
            // Only trigger when the target is an enemy
            return burnGA.Target is EnemyView;
        }
        return false;
    }

    public override void SubscribeCondition(Action<GameAction> reaction)
    {
        ActionSystem.SubscribeReaction<ApplyBurnGameAction>(reaction, reactionTiming);
    }

    public override void UnsubscribeCondition(Action<GameAction> reaction)
    {
        ActionSystem.UnsubscribeReaction<ApplyBurnGameAction>(reaction, reactionTiming);
    }
}
