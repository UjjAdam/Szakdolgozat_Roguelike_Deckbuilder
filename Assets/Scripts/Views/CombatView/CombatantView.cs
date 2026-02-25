using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.UI;

public class CombatantView : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private StatusEffectsUI statusEffectsUI;
    [SerializeField] private Slider healtbarslider;

    public int MaxHealth { get; private set; }

    public int CurrentHealth { get; private set; }

    private Dictionary<StatusEffectType, int> statusEffects = new();

    protected void SetupBase(int health, Sprite image)
    {
        MaxHealth = CurrentHealth = health;
        spriteRenderer.sprite = image;
        UpdateHealthText();
    }

    protected void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    private void UpdateHealthText()
    {
        healthText.text = "HP: " + CurrentHealth;
        float ratio = MaxHealth <= 0 ? 0f : (float)CurrentHealth / (float)MaxHealth;
        healtbarslider.value = Mathf.Clamp01(ratio);
    }

    // Allow restoring current health when a new HeroView instance is created for the same session hero.
    public void SetCurrentHealth(int health)
    {
        CurrentHealth = Mathf.Clamp(health, 0, MaxHealth);
        UpdateHealthText();

        if (this is HeroView)
            HeroSystem.Instance?.SaveHeroHealth(CurrentHealth);
    }

    public void Damage(int damageAmount)
    {
        int remainDamage = damageAmount;
        int currentArmor = GetStatusEffectStacks(StatusEffectType.ARMOR);
        if (currentArmor > 0)
        {
            if (currentArmor >= damageAmount)
            {
                RemoveStatusEffect(StatusEffectType.ARMOR, remainDamage);
                remainDamage = 0;
            }
            else if (currentArmor < damageAmount)
            {
                RemoveStatusEffect(StatusEffectType.ARMOR, currentArmor);
                remainDamage -= currentArmor;
            }
        }
        if (remainDamage > 0)
        {
            CurrentHealth -= remainDamage;
            if (CurrentHealth < 0)
            {
                CurrentHealth = 0;
            }
        }

        // Persist hero health immediately when hero is damaged
        if (this is HeroView)
        {
            HeroSystem.Instance?.SaveHeroHealth(CurrentHealth);
        }

        transform.DOShakePosition(0.2f, 0.5f);
        UpdateHealthText();
    }

    public void AddStatusEffect(StatusEffectType type, int stackCount)
    {
        if (statusEffects.ContainsKey(type))
        {
            statusEffects[type] += stackCount;
        }
        else
        {
            statusEffects.Add(type, stackCount);
        }

        // Defensive: statusEffectsUI may have been destroyed when this view was destroyed.
        if (statusEffectsUI != null)
        {
            statusEffectsUI.UpdateStatusEffectUI(type, GetStatusEffectStacks(type));
        }
    }

    public void RemoveStatusEffect(StatusEffectType type, int stackCount)
    {
        if (statusEffects.ContainsKey(type))
        {
            statusEffects[type] -= stackCount;
            if (statusEffects[type] <= 0)
            {
                statusEffects.Remove(type);
            }
        }

        // Defensive: statusEffectsUI may have been destroyed when this view was destroyed.
        if (statusEffectsUI != null)
        {
            statusEffectsUI.UpdateStatusEffectUI(type, GetStatusEffectStacks(type));
        }
    }
    public int GetStatusEffectStacks(StatusEffectType type)
    {
        if (statusEffects.ContainsKey(type))
        {
            return statusEffects[type];
        }
        else
        {
            return 0;
        }
    }
}
