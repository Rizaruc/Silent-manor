using UnityEngine;

public class KillPlayerTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DeathManager.instance.KillPlayer();
        }
    }
}
