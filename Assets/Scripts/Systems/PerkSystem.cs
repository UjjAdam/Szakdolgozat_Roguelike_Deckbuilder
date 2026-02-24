using System.Collections.Generic;
using UnityEngine;

public class PerkSystem : Singleton<PerkSystem>
{
    [SerializeField] private PerksUI perksUI; //set by PerksUI.Register
    [SerializeField] private PerkUI perkUIPrefab; // prefab used for preview
    private readonly List<Perk> perks = new();
    
    public void RegisterPerksUI(PerksUI ui)
    {
        perksUI = ui;
        // populate UI with existing perks if any
        foreach (var p in perks)
        {
            perksUI.AddPerkUI(p);
        }
    }

    public void UnregisterPerksUI(PerksUI ui)
    {
        if (perksUI == ui)
            perksUI = null;
    }

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

        // update UI only if present (Combat scene); Session/Chest can still add perks without PerksUI loaded
        perksUI?.AddPerkUI(perk);

        perk.OnAdd();
    }

    public void RemovePerk(Perk perk)
    {
        perks.Remove(perk);
        perksUI?.RemovePerkUI(perk);
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

    /// <summary>
    /// Instantiate a PerkUI preview under the given parent (used by Chest UI).
    /// Requires perkUIPrefab assigned in Inspector on the Session PerkSystem.
    /// Returns the created PerkUI so caller can position/destroy it as needed.
    /// </summary>
    public PerkUI CreatePerkPreview(Perk perk, Transform parent)
    {
        if (perk == null)
            return null;
        if (perkUIPrefab == null)
        {
            Debug.LogWarning("PerkSystem.CreatePerkPreview: perkUIPrefab is not assigned.");
            return null;
        }
        PerkUI ui = Instantiate(perkUIPrefab, parent);
        ui.Setup(perk);
        return ui;
    }

    
    
    // Read-only access to currently acquired perks.
    public IReadOnlyList<Perk> AcquiredPerks => perks.AsReadOnly();

    // Re-apply all perks' subscriptions
    public void ReapplyPerks()
    {
        foreach (var perk in perks)
        {
            // safe refresh: remove then add to ensure subscriptions are fresh
            try
            {
                perk.OnRemove();
            }
            catch { }
            try
            {
                perk.OnAdd();
            }
            catch { }
        }
    }
}
