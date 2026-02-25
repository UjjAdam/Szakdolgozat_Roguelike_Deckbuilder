using UnityEngine;

public class ShowRewardButton : MonoBehaviour
{
    [SerializeField] private GameObject RewardSystem;
    [SerializeField] private GameObject RewardSelectionUI;
    public void OnClick()
    {
        RewardSelectionUI.SetActive(true);

        RewardSystem.GetComponent<RewardSystem>().GenerateRewardCardsUI();

        this.gameObject.SetActive(false);
    }
}
