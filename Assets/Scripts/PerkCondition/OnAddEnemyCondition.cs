using System;
using UnityEngine;

public class OnAddEnemyCondition : PerkCondition
{
    public override bool SubConditionIsMet(GameAction gameAction)
    {
        //future: check if attacker is above x health ...
        return true;
    }

    public override void SubscribeCondition(Action<GameAction> reaction)
    {
        ActionSystem.SubscribeReaction<AddEnemyGameAction>(reaction, reactionTiming);
    }

    public override void UnsubscribeCondition(Action<GameAction> reaction)
    {
        ActionSystem.UnsubscribeReaction<AddEnemyGameAction>(reaction, reactionTiming);
    }
}
