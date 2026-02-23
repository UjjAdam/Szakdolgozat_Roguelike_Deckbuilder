using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealMenuUI : MonoBehaviour
{
    [SerializeField] private Slider targetSlider;
    [SerializeField] private GameObject healButton;
    public float duration = 3f; 

    public void StartSliderAnimation()
    {
        StartCoroutine(AnimateSlider());
    }

    IEnumerator AnimateSlider()
    {
        float elapsedTime = 0f;
        targetSlider.value = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
           
            targetSlider.value = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            yield return null;
        }

        if (targetSlider != null) targetSlider.value = 1f;

        // Heal the session-stored hero data to full without requiring a scene HeroView
        if (HeroSystem.Instance == null)
        {
            Debug.LogWarning("HealMenuUI: HeroSystem.Instance is null. Cannot heal.");
        }
        else
        {
            HeroSystem.Instance.HealSavedHeroToMax();
        }

        // hide the heal button (or make non-interactable as preferred)
        if (healButton != null)
            healButton.SetActive(false);
    }
}
