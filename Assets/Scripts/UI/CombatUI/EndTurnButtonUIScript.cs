using UnityEngine;

public class EndTurnButtonUIScript : MonoBehaviour
{
    public void OnClick()
    {
        EnemyTurnGameAction enemyTurnGA = new();
        ActionSystem.Instance.Perform(enemyTurnGA);
    }
}
