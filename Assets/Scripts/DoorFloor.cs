using UnityEngine;
using System.Collections;

public class DoorTeleporter : MonoBehaviour
{
    [Tooltip("Target transform to teleport the player to (set another door's transform).")]
    public Transform targetDoor;

    [Tooltip("Reference ke ScreenFader (drag dari Canvas)")]
    public ScreenFader screenFader;

    [Tooltip("Durasi fade (detik)")]
    public float fadeDuration = 0.25f;

    private bool playerInRange = false;
    private GameObject playerObject;
    private Rigidbody2D playerRb;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerObject = other.gameObject;
            playerRb = playerObject.GetComponent<Rigidbody2D>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerObject = null;
            playerRb = null;
        }
    }

    void Update()
    {
        if (playerInRange && playerObject != null && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(TeleportWithFade());
        }
    }

    private IEnumerator TeleportWithFade()
    {
        if (screenFader != null)
        {
            yield return screenFader.FadeOut(fadeDuration);
        }

        TeleportNow();

        if (screenFader != null)
        {
            yield return screenFader.FadeIn(fadeDuration);
        }
    }

    private void TeleportNow()
    {
        if (targetDoor == null || playerObject == null) return;

        if (playerRb != null)
            playerRb.linearVelocity = Vector2.zero;

        Vector3 newPos = targetDoor.position;
        newPos.z = playerObject.transform.position.z;
        playerObject.transform.position = newPos;

        Debug.Log($"Teleported to: {targetDoor.name}");
    }

    void OnDrawGizmosSelected()
    {
        if (targetDoor != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, targetDoor.position);
            Gizmos.DrawWireSphere(targetDoor.position, 0.15f);
        }
    }
}
