using UnityEngine;

public class ChestView : MonoBehaviour
{
    [SerializeField] private GameObject chestMenuUI;
    [SerializeField] private GameObject clickmeText;

    private void OnMouseDown()
    {
        chestMenuUI.SetActive(true);
        clickmeText.SetActive(false);
    }
}
