using UnityEngine;

public class AddRewardButton : MonoBehaviour
{
    [SerializeField] private RewardSystem rewardSystem;
    [SerializeField] private HeroData heroData;
    [SerializeField] private GameObject RewardSelectionUI;

    [SerializeField] private CardData cardData;

    public void OnClick()
    {
        rewardSystem.AddSpecificRewardToDeck(heroData, cardData);
        //Hide the reward UI
        RewardSelectionUI.SetActive(false);
    }

    public void SetCardData(CardData data)
    {
        cardData = data;
    }
}
