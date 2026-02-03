using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class FallToNextScene : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1.5f;
    public string nextSceneName;

    bool triggered = false;

    private void Start()
    {
        // Pastikan mulai transparan
        Color c = fadeImage.color;
        c.a = 0f;
        fadeImage.color = c;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            StartCoroutine(FadeAndLoad());
        }
    }

    IEnumerator FadeAndLoad()
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        SceneManager.LoadScene(nextSceneName);
    }
}
