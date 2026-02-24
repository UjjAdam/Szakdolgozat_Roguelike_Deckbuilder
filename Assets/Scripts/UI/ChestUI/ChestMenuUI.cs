using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ChestMenuUI : MonoBehaviour
{
    [SerializeField] private PerkData[] perkPool;          // PerkData pool
    [SerializeField] private Transform perkSlotParent;     // UI slot

    [SerializeField] private GameObject takePerkButton;
    [SerializeField] private GameObject continueButton;

    private Perk selectedPerk;
    private PerkUI previewUI;

    private void OnEnable()
    {
        GenerateRandomPerk();
    }

    private void OnDisable()
    {
        CleanupPreview();
    }

    private void GenerateRandomPerk()
    {
        CleanupPreview();

        if (perkPool == null || perkPool.Length == 0)
        {
            Debug.LogWarning("ChestMenuUI.GenerateRandomPerk: perkPool is empty.");
            selectedPerk = null;
            if (takePerkButton != null) takePerkButton.SetActive(false);
            if (continueButton != null) continueButton.SetActive(true);
            return;
        }

        // Build candidate list excluding perks the player already has (if PerkSystem available)
        PerkData[] candidates;
        if (PerkSystem.Instance != null)
        {
            var owned = PerkSystem.Instance.AcquiredPerks;
            candidates = perkPool.Where(d => !owned.Any(o => o.Source == d)).ToArray();
        }
        else
        {
            candidates = perkPool;
        }

        if (candidates.Length == 0)
        {
            // Player already has all perks in the pool -> nothing new to offer
            Debug.Log("ChestMenuUI: player already owns all perks from this chest pool.");
            selectedPerk = null;
            if (takePerkButton != null) takePerkButton.SetActive(false);
            if (continueButton != null) continueButton.SetActive(true);
            return;
        }

        // pick random PerkData from candidates and create Perk instance
        PerkData data = candidates[Random.Range(0, candidates.Length)];
        selectedPerk = new Perk(data);

        // create PerkUI preview using PerkSystem helper
        if (PerkSystem.Instance != null)
        {
            previewUI = PerkSystem.Instance.CreatePerkPreview(selectedPerk, perkSlotParent);
            if (previewUI != null)
            {
                // ensure correct rect transform defaults
                if (previewUI is Component c)
                {
                    RectTransform rt = c.GetComponent<RectTransform>();
                    if (rt != null)
                    {
                        rt.localPosition = Vector3.zero;
                        rt.localScale = Vector3.one;
                    }
                }
            }
        }
    }

    private void CleanupPreview()
    {
        if (previewUI != null)
        {
            Destroy(previewUI.gameObject);
            previewUI = null;
        }
    }

    public void TakePerk()
    {
        // add to player's persistent PerkSystem
        PerkSystem.Instance.AddPerk(selectedPerk);

        // update buttons
        takePerkButton.SetActive(false);
        continueButton.SetActive(true);
    }

}
