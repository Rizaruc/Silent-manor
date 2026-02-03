using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    [Header("Jumpscare")]
    public GameObject jumpscareUI;
    public AudioSource jumpscareAudio;
    public float restartDelay = 1.5f;

    private bool isDead = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (!collision.gameObject.CompareTag("Monster"))
            return;

        isDead = true;

        // 🔇 MATIKAN BGM
        if (BGMManager.Instance != null)
        {
            BGMManager.Instance.MuteBGMInstant();
            // atau:
            // BGMManager.Instance.FadeOutBGM(0.3f);
        }

        // Tampilkan jumpscare
        if (jumpscareUI != null)
            jumpscareUI.SetActive(true);

        // 🔊 Play jumpscare sound (TETAP ADA)
        if (jumpscareAudio != null)
            jumpscareAudio.Play();

        Invoke(nameof(RestartScene), restartDelay);
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
