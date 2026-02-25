using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardViewUI : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text energy;
    [SerializeField] private Image cardimage;

    private CardData cardData;
    private RewardSystem rewardSystem;
    private HeroData heroData;
    private RewardSelectionUI rewardSelectionUI;

    public void Setup(CardData data, RewardSystem rewardSystem, HeroData heroData, RewardSelectionUI rewardSelectionUI)
    {
        cardData = data;
        this.rewardSystem = rewardSystem;
        this.heroData = heroData;
        this.rewardSelectionUI = rewardSelectionUI;

        title.text = data.name;
        description.text = data.Description;
        energy.text = data.Energy.ToString();
        cardimage.sprite = data.Image;
    }

    public void OnAddToDeckButtonClicked()
    {
        if (rewardSystem != null && heroData != null && cardData != null)
        {
            rewardSystem.AddSpecificRewardToDeck(heroData, cardData);
        }
        if (rewardSelectionUI != null)
        {
            rewardSelectionUI.gameObject.SetActive(false);
        }
    }
}
