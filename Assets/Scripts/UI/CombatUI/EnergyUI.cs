using TMPro;
using UnityEngine;

public class EnergyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text energy;

    public void UpdateEnergyText(int currentEnergy)
    {
        energy.text = currentEnergy.ToString();
    }
}
