using UnityEngine;

public class HealStationView : MonoBehaviour
{
    [SerializeField] private GameObject healMenuUI;

    private void OnMouseDown()
    {
        healMenuUI.SetActive(true);
    }
}
