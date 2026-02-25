using UnityEngine;

public class CardViewCreator : Singleton<CardViewCreator>
{
    [SerializeField] private CardView cardViewprefab;

    // Expose whether prefab is assigned so callers can wait safely
    public bool HasPrefab => cardViewprefab != null;

    public CardView CreateCardView(Card card, Vector3 position, Quaternion rotation)
    {
        if (cardViewprefab == null)
        {
            Debug.LogWarning("CardViewCreator.CreateCardView: cardViewprefab is not assigned.");
            return null;
        }

        CardView cardView = Instantiate(cardViewprefab, position, rotation);
        cardView.Setup(card);
        return cardView;
    }
}
