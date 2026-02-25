using UnityEngine;

public class RewardSelectionUI : MonoBehaviour
{
    public static RewardSelectionUI Instance { get; private set; }

    [SerializeField] private Transform[] cardSlots;
    public void AddCardView(CardViewUI cardViewUI)
    {
        for (int i = 0; i < cardSlots.Length; i++)
        {
            if (cardSlots[i].childCount == 0)
            {
                cardViewUI.transform.SetParent(cardSlots[i], false);
                cardViewUI.transform.localPosition = Vector3.zero;
                cardViewUI.transform.localScale = Vector3.one;
                return;
            }
        }
        
    }

    public void ClearSlots()
    {
        foreach (var slot in cardSlots)
        {
            foreach (Transform child in slot)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
