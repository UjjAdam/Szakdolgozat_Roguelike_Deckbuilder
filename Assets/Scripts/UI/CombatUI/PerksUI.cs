using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PerksUI : MonoBehaviour
{
    [SerializeField] private PerkUI perkUIprefab;
    private readonly List<PerkUI> perkUIs = new();

    private void OnEnable()
    {
        // when Combat scene loads and this object enables, register with Session PerkSystem
        if (PerkSystem.Instance != null)
            PerkSystem.Instance.RegisterPerksUI(this);
    }

    private void OnDisable()
    {
        if (PerkSystem.Instance != null)
            PerkSystem.Instance.UnregisterPerksUI(this);
    }

    public void AddPerkUI(Perk perk)
    {
        PerkUI perkUI = Instantiate(perkUIprefab, transform);
        perkUI.Setup(perk);
        perkUIs.Add(perkUI);
    }

    public void RemovePerkUI(Perk perk)
    {
        PerkUI perkUI = perkUIs.Where(pui => pui.Perk == perk).FirstOrDefault();
        if (perkUI != null)
        {
            perkUIs.Remove(perkUI);
            Destroy(perkUI.gameObject);
        }
    }
}
