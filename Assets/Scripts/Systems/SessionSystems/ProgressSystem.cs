using System.Collections.Generic;
using UnityEngine;

public class ProgressSystem : Singleton<ProgressSystem>
{
    public int floorTracker = 1;

    // Tracks whether the current run has been initialized (starter deck applied, etc.)
    private bool runInitialized = false;

    // Cards the player acquired during the current run (do NOT store in HeroData asset)
    private readonly List<CardData> acquiredCards = new();

    public void IncreaseFloorNumber()
    {
        floorTracker++;
    }

    public bool IsRunInitialized => runInitialized;

    public void MarkRunInitialized()
    {
        runInitialized = true;
    }

    public void ResetRun()
    {
        floorTracker = 1;
        runInitialized = false;
        acquiredCards.Clear();
    }

    public void AddAcquiredCard(CardData cardData)
    {
        if (cardData == null) return;
        acquiredCards.Add(cardData);
    }

    public IReadOnlyList<CardData> GetAcquiredCards() => acquiredCards.AsReadOnly();
}
