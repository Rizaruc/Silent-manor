using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathManager : MonoBehaviour
{
    public static DeathManager instance;

    [Header("UI")]
    public Image blackScreen;
    public Image deathText;
    public Button tryAgainButton;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip deathScream;

    [Header("Timing / Speed")]
    public float blackFadeDuration = 0.3f;
    public float titleFadeDuration = 1.2f;
    public float buttonDelay = 0.5f;
    public float buttonFadeDuration = 0.6f;

    bool isDead = false; // 🔒 biar ga kepanggil 2x

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // STATE AWAL UI
        blackScreen.color = new Color(0, 0, 0, 0);
        deathText.color = new Color(1, 1, 1, 0);

        tryAgainButton.gameObject.SetActive(false);
        tryAgainButton.image.color = new Color(1, 1, 1, 0);
    }

    public void KillPlayer()
    {
        if (isDead) return; // anti double trigger
        isDead = true;

        // 🔇 MATIKAN SEMUA SOUND GAME
        if (AudioSilencer.instance != null)
            AudioSilencer.instance.SilenceAll();

        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        // 😱 SOUND TERIAK (TETAP NYALA)
        if (audioSource && deathScream)
            audioSource.PlayOneShot(deathScream);

        // FADE HITAM
        yield return StartCoroutine(
            FadeImage(blackScreen, 0f, 1f, blackFadeDuration)
        );

        // FADE JUDUL
        yield return StartCoroutine(
            FadeImage(deathText, 0f, 1f, titleFadeDuration)
        );

        // DELAY SEBELUM TOMBOL
        yield return new WaitForSeconds(buttonDelay);

        // TOMBOL FADE IN
        tryAgainButton.gameObject.SetActive(true);
        yield return StartCoroutine(
            FadeImage(tryAgainButton.image, 0f, 1f, buttonFadeDuration)
        );
    }

    IEnumerator FadeImage(Image img, float from, float to, float duration)
    {
        float t = 0f;
        Color c = img.color;

        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(from, to, t / duration);
            img.color = c;
            yield return null;
        }

        c.a = to;
        img.color = c;
    }

    public void TryAgain()
    {
        // 🔊 HIDUPKAN LAGI SOUND
        if (AudioSilencer.instance != null)
            AudioSilencer.instance.ResumeAll();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
