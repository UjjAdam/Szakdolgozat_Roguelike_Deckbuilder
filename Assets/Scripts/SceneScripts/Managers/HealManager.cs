using DG.Tweening;
using UnityEngine;

public class HealManager : MonoBehaviour
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
        SoftReset();

        SceneController.Instance
            .NewTransition()
            .Load(SceneDatabase.Slots.SessionContent, SceneDatabase.Scenes.Travelsal, setActive: true)
            .UnLoad(SceneDatabase.Slots.SessionContent)
            .WithLoading()
            .Perform();
    }

    private void ResetEverything()
    {
        SoftReset();

        CardSystem.Instance.ResetDeck();
        EnemySystem.Instance.Reset();
        PerkSystem.Instance.Reset();
        ActionSystem.Instance.Reset();

        ProgressSystem.Instance?.ResetRun();
    }

    private void SoftReset()
    {
        Time.timeScale = 1f;

        DOTween.KillAll();
        DOTween.Clear(true);
    }
}
