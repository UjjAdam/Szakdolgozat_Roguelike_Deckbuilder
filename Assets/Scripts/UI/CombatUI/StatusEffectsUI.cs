using System.Collections.Generic;
using UnityEngine;

public class StatusEffectsUI : MonoBehaviour
{
    [SerializeField] private StatusEffectUI statusEffectsUIprefab;
    [SerializeField] private Sprite armorSprite, burnSprite;
    private Dictionary<StatusEffectType, StatusEffectUI> statusEffectUIs = new();

    public void UpdateStatusEffectUI(StatusEffectType statusEffectType, int stackCount)
    {
        // Defensive guards: prefab may be missing or this UI may itself be destroyed.
        if (this == null || gameObject == null) // shot-in-the-foot guard
            return;

        // If we need to remove, ensure key exists and target UI not already destroyed.
        if (stackCount == 0)
        {
            if (statusEffectUIs.ContainsKey(statusEffectType))
            {
                StatusEffectUI statusEffectUI = statusEffectUIs[statusEffectType];
                statusEffectUIs.Remove(statusEffectType);
                if (statusEffectUI != null)
                    Destroy(statusEffectUI.gameObject);
            }
            return;
        }

        // Ensure we have a prefab to instantiate
        if (statusEffectsUIprefab == null)
        {
            Debug.LogWarning("StatusEffectsUI.UpdateStatusEffectUI: statusEffectsUIprefab is not assigned.");
            return;
        }

        // If UI for this type doesn't exist or was destroyed, create it
        if (!statusEffectUIs.ContainsKey(statusEffectType) || statusEffectUIs[statusEffectType] == null)
        {
            // Remove stale null entry if present
            if (statusEffectUIs.ContainsKey(statusEffectType) && statusEffectUIs[statusEffectType] == null)
                statusEffectUIs.Remove(statusEffectType);

            StatusEffectUI statusEffectUI = Instantiate(statusEffectsUIprefab, transform);
            statusEffectUIs[statusEffectType] = statusEffectUI;
        }

        Sprite sprite = GetSpriteByType(statusEffectType);
        var ui = statusEffectUIs[statusEffectType];
        if (ui != null)
            ui.Set(sprite, stackCount);
    }

    private Sprite GetSpriteByType(StatusEffectType statusEffectType)
    {
        return statusEffectType switch
        {
            StatusEffectType.ARMOR => armorSprite,
            StatusEffectType.BURN => burnSprite,
            _ => null,
        };
    }
}
