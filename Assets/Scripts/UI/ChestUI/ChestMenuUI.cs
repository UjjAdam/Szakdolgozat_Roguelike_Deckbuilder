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

        // pick random PerkData and create Perk instance
        PerkData data = perkPool[Random.Range(0, perkPool.Length)];
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
