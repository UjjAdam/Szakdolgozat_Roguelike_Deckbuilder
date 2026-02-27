using System.Collections.Generic;
using System.Linq;
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


        // If ProgressSystem is available and floorTracker == 10 -> force both entries to be COMBAT
        var progress = FindFirstObjectByType<ProgressSystem>();
        if (progress != null && progress.floorTracker == 10)
        {
            var combats = caveDatas.Where(d => d.EntryType == CaveEntryType.COMBAT).ToList();
            if (combats.Count == 0)
            {
                Debug.LogWarning("TravelsalSetupSystem: no COMBAT entries found in caveDatas for floor 10, falling back to normal selection.");
                // fallthrough to normal selection below
            }
            else if (combats.Count == 1)
            {
                // only one COMBAT available -> add it twice
                system.AddCaveEntry(new CaveEntry(combats[0]));
                system.AddCaveEntry(new CaveEntry(combats[0]));
                return;
            }
            else
            {
                // choose two distinct COMBAT entries
                int totalCombats = combats.Count;
                int first = Random.Range(0, totalCombats);
                int second = Random.Range(0, totalCombats - 1);
                if (second >= first) second++;
                system.AddCaveEntry(new CaveEntry(combats[first]));
                system.AddCaveEntry(new CaveEntry(combats[second]));
                return;
            }
        }

        // If ProgressSystem is available and floorTracker == 11 -> force both entries to be EXIT
        if (progress != null && progress.floorTracker == 11)
        {
            var exits = caveDatas.Where(d => d.EntryType == CaveEntryType.EXIT).ToList();
            if (exits.Count == 0)
            {
                Debug.LogWarning("TravelsalSetupSystem: no EXIT entries found in caveDatas, falling back to normal selection.");
                // fallthrough to normal selection below
            }
            else if (exits.Count == 1)
            {
                // only one EXIT available -> add it twice
                system.AddCaveEntry(new CaveEntry(exits[0]));
                system.AddCaveEntry(new CaveEntry(exits[0]));
                return;
            }
            else
            {
                // choose two distinct EXIT entries
                int totalExits = exits.Count;
                int first = Random.Range(0, totalExits);
                int second = Random.Range(0, totalExits - 1);
                if (second >= first) second++;
                system.AddCaveEntry(new CaveEntry(exits[first]));
                system.AddCaveEntry(new CaveEntry(exits[second]));
                return;
            }
        }

        // Normal behavior: pick two distinct random entries EXCLUDING EXIT type when possible
        var nonExitList = caveDatas.Where(d => d.EntryType != CaveEntryType.EXIT).ToList();

        // If we have at least two non-exit entries, pick two distinct from them
        if (nonExitList.Count >= 2)
        {
            int total = nonExitList.Count;
            int firstIndex = Random.Range(0, total);
            int secondIndex = Random.Range(0, total - 1);
            if (secondIndex >= firstIndex) secondIndex++;
            system.AddCaveEntry(new CaveEntry(nonExitList[firstIndex]));
            system.AddCaveEntry(new CaveEntry(nonExitList[secondIndex]));
            return;
        }

        // If only one non-exit entry exists, add it and pick another from the remaining pool
        if (nonExitList.Count == 1)
        {
            system.AddCaveEntry(new CaveEntry(nonExitList[0]));

            // build fallback pool excluding the already added one
            var fallback = caveDatas.Where(d => d != nonExitList[0]).ToList();
            if (fallback.Count == 0)
            {
                // duplicate the single non-exit if nothing else exists
                system.AddCaveEntry(new CaveEntry(nonExitList[0]));
            }
            else
            {
                var pick = fallback[Random.Range(0, fallback.Count)];
                system.AddCaveEntry(new CaveEntry(pick));
            }
            return;
        }

        // If no non-exit entries available, fall back to picking any two from the full list
        int totalAll = caveDatas.Count;
        if (totalAll <= 2)
        {
            foreach (var caveData in caveDatas)
                system.AddCaveEntry(new CaveEntry(caveData));
            return;
        }

        int firstIdxAll = Random.Range(0, totalAll);
        int secondIdxAll = Random.Range(0, totalAll - 1);
        if (secondIdxAll >= firstIdxAll) secondIdxAll++;
        system.AddCaveEntry(new CaveEntry(caveDatas[firstIdxAll]));
        system.AddCaveEntry(new CaveEntry(caveDatas[secondIdxAll]));
    }
}
