using UnityEngine;

public class HealStationView : MonoBehaviour
{
    [SerializeField] private GameObject healMenuUI;
    [SerializeField] private GameObject clickmeText;

    private void OnMouseDown()
    {
        healMenuUI.SetActive(true);
        clickmeText.SetActive(false);
    }
}
