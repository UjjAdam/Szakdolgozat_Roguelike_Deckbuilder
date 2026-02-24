using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CaveEntry
{
    public Sprite Image => data.Image;
    public CaveEntryType type=> data.EntryType;
    public string Description => data.Description;
    private readonly CaveEntryData data;

    public CaveEntry(CaveEntryData caveData)
    {
        data = caveData;
    }

}
