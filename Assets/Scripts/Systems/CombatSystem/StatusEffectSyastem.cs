using System.Collections;
using UnityEngine;

public class StatusEffectSyastem : MonoBehaviour
{
    private void OnEnable()
    {
        ActionSystem.AttachPerformer<AddStatusEffectGameAction>(AddStatusEffectPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<AddStatusEffectGameAction>();
    }

    private IEnumerator AddStatusEffectPerformer(AddStatusEffectGameAction addStatusEffectGA)
    {
        foreach (var target in addStatusEffectGA.Targets)
        {
            target.AddStatusEffect(addStatusEffectGA.StatusEffectType, addStatusEffectGA.StackCount);
            yield return null;
            //Future ADD VFX for animation
        }
    }
}
