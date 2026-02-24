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
        var system = CaveEntrySystem.Instance;

        int total = caveDatas.Count;

        // Pick two distinct random indices (without replacement).
        int firstIndex = Random.Range(0, total);
        int secondIndex = Random.Range(0, total - 1);
        if (secondIndex >= firstIndex) secondIndex++;

        system.AddCaveEntry(new CaveEntry(caveDatas[firstIndex]));
        system.AddCaveEntry(new CaveEntry(caveDatas[secondIndex]));
    }
}
