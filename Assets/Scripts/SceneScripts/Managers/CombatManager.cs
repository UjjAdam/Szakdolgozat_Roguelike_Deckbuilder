using DG.Tweening;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public void SessionEnd()
    {
        ResetEverything();

        SceneController.Instance
            .NewTransition()
            .Load(SceneDatabase.Slots.Menu, SceneDatabase.Scenes.MainMenu, setActive: true)
            .UnLoad(SceneDatabase.Slots.Session)
            .UnLoad(SceneDatabase.Slots.SessionContent)
            .WithLoading()
            .WithClearUnusedAssets()
            .Perform();
    }

    public void SwitchToTravelsal()
    {

    }

    private void ResetEverything()
    {
        Time.timeScale = 1f;

        DOTween.KillAll();
        DOTween.Clear(true);

        CardSystem.Instance.ResetDeck();
        EnemySystem.Instance.Reset();
        PerkSystem.Instance.Reset();
        ActionSystem.Instance.Reset();
    }
}
