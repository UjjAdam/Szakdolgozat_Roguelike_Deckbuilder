using System.Collections;
using UnityEngine;

public class EffectSystem : MonoBehaviour
{
    public void OnEnable()
    {
        ActionSystem.AttachPerformer<PerformEffectGameAction>(PerformEffectPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<PerformEffectGameAction>();
    }

    // Performer

    private IEnumerator PerformEffectPerformer(PerformEffectGameAction performEffectGA)
    {
        GameAction effectAction = performEffectGA.Effect.GetGameAction(performEffectGA.Targets, HeroSystem.Instance.HeroView);
        ActionSystem.Instance.AddReaction(effectAction);
        yield return null;
    }
}
