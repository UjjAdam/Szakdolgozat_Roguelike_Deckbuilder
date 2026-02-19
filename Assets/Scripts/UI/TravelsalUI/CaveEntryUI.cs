using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CaveEntryUI : MonoBehaviour//,IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image image;
    [SerializeField] private Button travelButton; 
    public CaveEntry CaveEntry { get; private set; }

    private void Awake()
    {
        if (travelButton != null)
        {
           
            travelButton.onClick.RemoveAllListeners();
            travelButton.onClick.AddListener(OnTravelButtonClicked);
        } 
    }

    public void Setup(CaveEntry cave)
    {
        CaveEntry = cave;
        image.sprite = cave.Image;
        //description.SetText(perk.Description);
    }

    private void OnTravelButtonClicked()
    {
        TraversalManager manager = FindFirstObjectByType<TraversalManager>();
        if (manager == null)
        {
            Debug.LogWarning("TraversalManager not found in scene. Make sure a TraversalManager exists.");
            return;
        }

        manager.SelectAndTravel(this);
    }

    /* leírás doboz
    public void OnPointerEnter(PointerEventData eventData)
    {
        Descriptionbox.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Descriptionbox.SetActive(false);
    }
    */
}
