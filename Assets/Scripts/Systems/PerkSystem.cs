using System.Collections.Generic;
using UnityEngine;

public class PerkSystem : Singleton<PerkSystem>
{
    [SerializeField] private PerksUI perksUI;
    private readonly List<Perk> perks = new();
    
    public void AddPerk(Perk perk)
    {
        if (perk == null || perk.Source == null)
        {
            Debug.LogWarning("PerkSystem.AddPerk: null perk or missing PerkData.");
            return;
        }

        // Prevent adding the same PerkData twice
        if (perks.Exists(p => p.Source == perk.Source))
        {
            Debug.Log($"PerkSystem: perk '{perk.Source.name}' already added — skipping duplicate.");
            return;
        }

        perks.Add(perk);
        perksUI.AddPerkUI(perk);
        perk.OnAdd();
    }

    public void RemovePerk(Perk perk)
    {
        perks.Remove(perk);
        perksUI.RemovePerkUI(perk);
        perk.OnRemove();
    }

    public void Reset()
    {
        foreach (var perk in perks)
        {
            perk.OnRemove();
        }
        perks.Clear();
    }

    // Ensure subscriptions are removed when the system is disabled/destroyed
    private void OnDisable()
    {
        Reset();
    }
}
