using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PerkUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image image;
    [SerializeField] private GameObject Descriptionbox;
    [SerializeField] private TMP_Text description;
    public Perk Perk { get; private set; }

    public void Setup (Perk perk)
    {
        Perk = perk;
        image.sprite = perk.Image;
        description.SetText(perk.Description);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Descriptionbox.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Descriptionbox.SetActive(false);
    }
}
