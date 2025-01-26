using System.Collections;
using TMPro;
using UnityEngine;

public class creditosScript : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private TMP_Text[] textGroups;
    [SerializeField] private GameObject finalObjects;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float displayDuration = 2f;

    private void Start()
    {
        StartCoroutine(PlayCredits());
    }

    private IEnumerator PlayCredits()
    {

        background.SetActive(true);

        foreach (var text in textGroups)
        {
            yield return StartCoroutine(FadeInText(text));
            yield return new WaitForSeconds(displayDuration);
            yield return StartCoroutine(FadeOutText(text));
        }


        finalObjects.SetActive(true);
    }

    private IEnumerator FadeInText(TMP_Text text)
    {
        float elapsedTime = 0f;
        Color color = text.color;
        color.a = 0f;
        text.color = color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            text.color = color;
            yield return null;
        }

        color.a = 1f;
        text.color = color;
    }

    private IEnumerator FadeOutText(TMP_Text text)
    {
        float elapsedTime = 0f;
        Color color = text.color;
        color.a = 1f;
        text.color = color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(1f - (elapsedTime / fadeDuration));
            text.color = color;
            yield return null;
        }

        color.a = 0f;
        text.color = color;
    }
}

