using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
    public static FadeUI Instance;
    public Image fadePanel;             

    void Awake()
    {
        // シングルトンパターンの実装
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void StartFadeOut(float fadeDuration)
    {
        StartCoroutine(FadeOut(fadeDuration));
    }

    private IEnumerator FadeOut(float fadeDuration)
    {
        Color fadingcolor = new Color(0,0,0,1);

        float startAlpha = fadePanel.color.a;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            fadingcolor.a = Mathf.Lerp(startAlpha, 1, elapsed / fadeDuration);

            fadePanel.color = fadingcolor;
            yield return null;
        }

        fadePanel.color = new Color(0, 0, 0, 0);
    }
}
