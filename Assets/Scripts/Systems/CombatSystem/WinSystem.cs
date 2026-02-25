using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WinSystem : MonoBehaviour
{
    [SerializeField] private GameObject winUI;

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<WinGameAction>(WinPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<WinGameAction>();
    }

    private IEnumerator WinPerformer(WinGameAction winGA)
    {
        winUI.SetActive(true);
        yield break;
    }
}
