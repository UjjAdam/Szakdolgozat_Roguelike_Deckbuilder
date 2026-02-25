using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] GameObject PauseScreen;
    public void EnablePauseMenu()
    {
        PauseScreen.SetActive(true);
    }

    public void DisablePauseMenu()
    {
        PauseScreen.SetActive(false);
    }
}
