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

        // HANYA JIKA NUBRUK MONSTER
        if (!collision.gameObject.CompareTag("Monster"))
            return;

        isDead = true;

        // tampilkan jumpscare
        if (jumpscareUI != null)
            jumpscareUI.SetActive(true);

        // mainkan suara teriak
        if (jumpscareAudio != null)
            jumpscareAudio.Play();

        Invoke(nameof(RestartScene), restartDelay);
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    //Ruslan Abdul 
}
