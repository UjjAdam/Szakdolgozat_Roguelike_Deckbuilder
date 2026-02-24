using DG.Tweening;
using UnityEngine;

public class ExitManager : MonoBehaviour
{
    public void SessionEnd()
    {
        SceneController.Instance
            .NewTransition()
            .Load(SceneDatabase.Slots.Menu, SceneDatabase.Scenes.MainMenu, setActive: true)
            .UnLoad(SceneDatabase.Slots.Session)
            .UnLoad(SceneDatabase.Slots.SessionContent)
            .WithLoading()
            .WithClearUnusedAssets()
            .Perform();
    }
}
