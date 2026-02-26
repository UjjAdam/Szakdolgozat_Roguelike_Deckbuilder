using System;
using UnityEngine;

public class OnCardDrawCondition : PerkCondition
{
    public override bool SubConditionIsMet(GameAction gameAction)
    {
        //future: check if attacker is above x health ...
        return true;
    }

    public override void SubscribeCondition(Action<GameAction> reaction)
    {
        ActionSystem.SubscribeReaction<DrawCardsGameAction>(reaction, reactionTiming);
    }

    public override void UnsubscribeCondition(Action<GameAction> reaction)
    {
        ActionSystem.UnsubscribeReaction<DrawCardsGameAction>(reaction, reactionTiming);
    }
}
