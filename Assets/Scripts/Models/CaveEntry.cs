using System.Collections.Generic;
using UnityEngine;

public class CaveEntry
{
    public Sprite Image => data.Image;
    public CaveEntryType type=> data.EntryType; 
    private readonly CaveEntryData data;

    public CaveEntry(CaveEntryData caveData)
    {
        data = caveData;
    }

}
