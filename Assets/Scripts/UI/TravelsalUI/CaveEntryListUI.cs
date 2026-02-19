using System.Collections.Generic;
using UnityEngine;

public class CaveEntryListUI : MonoBehaviour
{
    [SerializeField] private CaveEntryUI caveentryUIprefab;
    private readonly List<CaveEntryUI> caveentryUIs = new();

    public void AddCaveEntryUI(CaveEntry cave)
    {
        CaveEntryUI caveUI = Instantiate(caveentryUIprefab, transform);
        caveUI.Setup(cave);
        caveentryUIs.Add(caveUI);
    }
    
}
