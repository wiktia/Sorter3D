using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeSpeed = 1f;
    private bool isFading;

    private void Start()
    {
        
        StartCoroutine(FadeIn());
    }

    public void FadeToLevel(string sceneName)
    {
        if (!isFading)
        {
            StartCoroutine(FadeOutAndLoad(sceneName)); 
        }
    }

    private IEnumerator FadeIn()
    {
        isFading = true;
        fadeImage.color = new Color(0, 0, 0, 1); // czarny

        while (fadeImage.color.a > 0)
        {
            fadeImage.color = new Color(0, 0, 0, fadeImage.color.a - (fadeSpeed * Time.deltaTime));
            yield return null;
        }

        fadeImage.gameObject.SetActive(false); // wyłączamy Image 
        isFading = false;
    }

    private IEnumerator FadeOutAndLoad(string sceneName) 
    {
        fadeImage.gameObject.SetActive(true); // włączamy Image przed 
        isFading = true;
        fadeImage.color = new Color(0, 0, 0, 0); // ustawiamy początkową przezroczystość

        while (fadeImage.color.a < 1)
        {
            fadeImage.color = new Color(0, 0, 0, fadeImage.color.a + (fadeSpeed * Time.deltaTime));
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}