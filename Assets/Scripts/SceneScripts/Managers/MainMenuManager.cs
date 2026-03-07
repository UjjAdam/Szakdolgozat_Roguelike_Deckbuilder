using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject Tutorialpage1;
    [SerializeField] private GameObject Tutorialpage2;
    [SerializeField] private GameObject Tutorialpage3;
    [SerializeField] private GameObject Tutorialpage4;
    [SerializeField] private GameObject Tutorialplane;
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

    public void BeginTutorial()
    {
        Tutorialplane.SetActive(true);
    }

    public void Next1()
    {
        Tutorialpage1.SetActive(false);
        Tutorialpage2.SetActive(true);
    }
    public void Next2()
    {
        Tutorialpage2.SetActive(false);
        Tutorialpage3.SetActive(true);
    }
    public void Next3()
    {
        Tutorialpage3.SetActive(false);
        Tutorialpage4.SetActive(true);
    }
    public void ExitTutorial()
    {
        Tutorialplane.SetActive(false);
        Tutorialpage4.SetActive(false);
        Tutorialpage1.SetActive(true);
    }
}
