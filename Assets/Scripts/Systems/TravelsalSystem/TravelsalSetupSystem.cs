using System.Collections.Generic;
using UnityEngine;

public class TravelsalSetupSystem : MonoBehaviour
{
    [SerializeField] private List<CaveEntryData> caveDatas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AddCaveEntry();
    }

    private void AddCaveEntry()
    {
        //hozzáadja az összes cavet a caveDatas-ból
        if (caveDatas != null)
        {
            foreach (var caveData in caveDatas)
            {
                CaveEntrySystem.Instance.AddCaveEntry(new CaveEntry(caveData));
            }
        }
    }
}
