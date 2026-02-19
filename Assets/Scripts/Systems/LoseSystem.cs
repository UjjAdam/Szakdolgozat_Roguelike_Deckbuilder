using System.Collections;
using UnityEngine;

public class LoseSystem : MonoBehaviour
{
    [SerializeField] private GameObject loseUI;

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<LoseGameAction>(LosePerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<LoseGameAction>();
    }

    private IEnumerator LosePerformer(LoseGameAction loseGA)
    {
        loseUI.SetActive(true);

        // Most állítjuk meg a játékot, csak miután a LoseGameAction performere lefutott
        Time.timeScale = 0f;

        yield break;
    }
}
