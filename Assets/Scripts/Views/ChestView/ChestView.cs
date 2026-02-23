using UnityEngine;

public class ChestView : MonoBehaviour
{
    [SerializeField] private GameObject chestMenuUI;

    private void OnMouseDown()
    {
        chestMenuUI.SetActive(true);
    }
}
