using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void SessionStart()
    {
        SceneController.Instance
            .NewTransition()
            .Load(SceneDatabase.Slots.Session, SceneDatabase.Scenes.Session)
            .Load(SceneDatabase.Slots.SessionContent, SceneDatabase.Scenes.Combat, setActive: true)
            .UnLoad(SceneDatabase.Slots.Menu)
            .WithLoading()
            .WithClearUnusedAssets()
            .Perform();
    }

    public void Kilepes()
    {
        Application.Quit();
    }
}
