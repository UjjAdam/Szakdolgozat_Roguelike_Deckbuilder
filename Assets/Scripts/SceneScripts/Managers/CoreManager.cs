using UnityEngine;

public class CoreManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Core Setup
        //Betölteni mindent mint pl SaveSystem vagy Audio
        SceneController.Instance
            .NewTransition()
            .Load(SceneDatabase.Slots.Menu, SceneDatabase.Scenes.MainMenu)
            .WithClearUnusedAssets()
            .Perform();
            
    }

}
