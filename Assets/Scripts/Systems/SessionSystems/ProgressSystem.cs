using UnityEngine;

public class ProgressSystem : Singleton<ProgressSystem>
{
    public int floorTracker = 1;

    public void IncreaseFloorNumber()
    { 
        floorTracker++; 
    }
}
