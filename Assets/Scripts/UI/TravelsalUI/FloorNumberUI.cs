using TMPro;
using UnityEngine;

public class FloorNumberUI : MonoBehaviour
{
    [SerializeField] private TMP_Text floorCounterText;
    public ProgressSystem progressSystem { get; private set; }

    void Start()
    {
        progressSystem = FindFirstObjectByType<ProgressSystem>();
        floorCounterText.SetText(progressSystem.floorTracker.ToString());
    }

    public void IncreaseFloorWhenLeaving()
    {
        progressSystem.IncreaseFloorNumber();
    }
}
