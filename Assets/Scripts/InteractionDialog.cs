using UnityEngine;

public class InteractionDialog : MonoBehaviour
{
    public string[] dialogLines;

    private bool playerNear = false;
    private bool dialogStarted = false; // ✅ PENTING

    void Update()
    {
        if (playerNear && !dialogStarted && Input.GetKeyDown(KeyCode.E))
        {
            dialogStarted = true;
            DialogManager.Instance.StartDialog(dialogLines);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNear = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            dialogStarted = false; // ✅ reset kalau player pergi
        }
    }
}
