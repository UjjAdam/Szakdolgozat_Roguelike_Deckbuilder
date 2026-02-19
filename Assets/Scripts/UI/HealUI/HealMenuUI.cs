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

        targetSlider.value = 1f;

        healButton.SetActive(false);
    }
}
