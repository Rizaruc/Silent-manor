using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FallToNextScene : MonoBehaviour
{
    public SpriteRenderer fadeSprite;   // ← sprite hitam
    public float fadeDuration = 1.5f;
    public float blackScreenHold = 0.5f;
    public string nextSceneName;

    private bool triggered = false;

    void Start()
    {
        // Pastikan mulai transparan
        Color c = fadeSprite.color;
        c.a = 0f;
        fadeSprite.color = c;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;

            Player player = other.GetComponent<Player>();
            if (player != null)
                player.SetCanMove(false);

            StartCoroutine(FadeAndLoad());
        }
    }

    IEnumerator FadeAndLoad()
    {
        float t = 0f;
        Color c = fadeSprite.color;

        // Fade in ke hitam total
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadeSprite.color = c;
            yield return null;
        }

        // Tahan hitam
        yield return new WaitForSeconds(blackScreenHold);

        SceneManager.LoadScene(nextSceneName);
    }
}
