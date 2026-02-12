using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyView : CombatantView
{
    [SerializeField] TMP_Text attackText;
    [SerializeField] Image defaultattackIndicator;

    [SerializeField] Image burnIndicator;
    [SerializeField] Image shieldIndicator;
    public int AttackPower { get; set; }
    public int ShieldPower { get; private set; }
    public int BurnPower { get; private set; }

    public void Setup(EnemyData enemyData)
    {
        AttackPower = enemyData.AttackPower;
        ShieldPower = enemyData.ShieldPower;
        BurnPower = enemyData.BurnPower;
        UpdateAttackText();
        SetupBase(enemyData.Health, enemyData.Image);
    }

    private void UpdateAttackText()
    {
        if (AttackPower != 0)
        {
            attackText.text = "Attack: " + AttackPower;
        }
        else if (ShieldPower != 0)
        {
            attackText.text = "Shield: " + ShieldPower;
            defaultattackIndicator.gameObject.SetActive(false);
            shieldIndicator.gameObject.SetActive(true);
        }
        else if(BurnPower != 0)
        {
            attackText.text = "Burn: " + BurnPower;
            defaultattackIndicator.gameObject.SetActive(false);
            burnIndicator.gameObject.SetActive(true);
        }
    }
}
