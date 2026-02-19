using System.Collections.Generic;
using UnityEngine;

public class CaveEntrySystem : Singleton<CaveEntrySystem>
{
    [SerializeField] private CaveEntryListUI caveEntryListUI;
    private readonly List<CaveEntry> caves = new();

    public void AddCaveEntry(CaveEntry cave)
    {
        caves.Add(cave);
        caveEntryListUI.AddCaveEntryUI(cave);
    }
}
