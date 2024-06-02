using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiePanel : MonoBehaviour
{
    [SerializeField] private Image youDiedImage;
    [SerializeField] private TextMeshProUGUI youDiedText; 
    [SerializeField] private GameObject returnButton; 
    [SerializeField] private float duration = 2.0f;

    private void OnEnable()
    {
        CharacterHealth.OnDieEvent += DiedPanelAnimation;
    }

    private void OnDisable()
    {
        CharacterHealth.OnDieEvent -= DiedPanelAnimation;
        
    }
    private void DiedPanelAnimation()
    {
        youDiedImage.gameObject.SetActive(true);
        if (youDiedImage != null)
        {
            StartCoroutine(FadeInImage(youDiedImage,youDiedText, duration));
        }
    }

    private IEnumerator FadeInImage(Image image,TextMeshProUGUI text, float duration)
    {
        Color imageColor = image.color;
        Color textColor = text.color;
        float startAlpha = 0f;
        float endAlpha = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            imageColor.a = alpha;
            textColor.a = alpha;
            image.color = imageColor;
            text.color = textColor;
            yield return null;
        }
        imageColor.a = endAlpha;
        textColor.a = endAlpha;
        image.color = imageColor;
        text.color = textColor;
        OnComplete();
    }

    private void OnComplete()
    {
        returnButton.SetActive(true);
    }
}