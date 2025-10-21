using System.Collections.Generic;
using UnityEngine;

public class MatchSetupSystem : MonoBehaviour
{
    [SerializeField] private HeroData heroData;
    [SerializeField] private List<PerkData> perkDatas;
    [SerializeField] private List<EnemyData> enemyDatas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        CardSystem.Instance.ResetDeck();
        HeroSystem.Instance.Setup(heroData);
        EnemySystem.Instance.Setup(enemyDatas);
        CardSystem.Instance.Setup(heroData.StarterDeck);

        //hozzáadja az összes perket a perkDatas-ból mivel, azért kell így mert csak 1 perket ad hozzá a System egyszerre
        if (perkDatas != null)
        {
            foreach (var perkData in perkDatas)
            {
                PerkSystem.Instance.AddPerk(new Perk(perkData));
            }
        }


        DrawCardsGameAction drawCardsGA = new(5);
        ActionSystem.Instance.Perform(drawCardsGA);
    }

}
