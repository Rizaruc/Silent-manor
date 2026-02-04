using UnityEngine;
using TMPro;

public class DoorInteract : MonoBehaviour
{
    public GameObject hintText; // UI Text
    bool playerNearby = false;

    void Start()
    {
        hintText.SetActive(false);
    }

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Masuk pintu 🚪");
            // nanti di sini kamu sambung ke teleport / fade
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            hintText.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            hintText.SetActive(false);
        }
    }
}
