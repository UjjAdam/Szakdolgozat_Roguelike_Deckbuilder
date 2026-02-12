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
        CardSystem.Instance.Setup(heroData.StarterDeck);
        //régi EnemySystem.Instance.Setup(enemyDatas);
        GenerateEnemies();
        AddPerks();
        DrawCardsGameAction drawCardsGA = new(5);
        ActionSystem.Instance.Perform(drawCardsGA);
    }

    private void GenerateEnemies()
    {
        // random enemy az enemyDatas-ból
        List<EnemyData> enemiesToSpawn = new List<EnemyData>();
        if (enemyDatas != null && enemyDatas.Count > 0)
        {
            int count = Random.Range(1, 4);
            for (int i = 0; i < count; i++)
            {
                EnemyData chosen = enemyDatas[Random.Range(0, enemyDatas.Count)];
                enemiesToSpawn.Add(chosen);
            }
        }

        EnemySystem.Instance.Setup(enemiesToSpawn);

    }

    private void AddPerks()
    {
        //hozzáadja az összes perket a perkDatas-ból mivel, azért kell így mert csak 1 perket ad hozzá a System egyszerre
        if (perkDatas != null)
        {
            foreach (var perkData in perkDatas)
            {
                PerkSystem.Instance.AddPerk(new Perk(perkData));
            }
        }
    }

}
