using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    [Header("Interact settings")]
    public float interactRange = 1.2f;            // seberapa jauh player bisa interact
    public LayerMask interactMask;                // set ke layer Interactable

    [Header("UI")]
    public Text interactText;                     // "E to Interact" (UI Text)
    public RectTransform interactTextRect;        // rect transform dari interactText (untuk pos)
    public Text infoText;                         // "Need a Key"

    [Header("Feedback")]
    public float infoDuration = 1.2f;

    bool hasKey = false;
    Transform currentTarget;

    Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
        if (interactText != null) interactText.gameObject.SetActive(false);
        if (infoText != null) infoText.gameObject.SetActive(false);
    }

    void Update()
    {
        CheckForInteractable();
        UpdateInteractTextPosition();

        if (currentTarget != null && Input.GetKeyDown(KeyCode.E))
        {
            if (currentTarget.CompareTag("Key"))
            {
                PickUpKey();
            }
            else if (currentTarget.CompareTag("Door"))
            {
                TryOpenDoor();
            }
        }
    }

    void CheckForInteractable()
    {
        // cek overlap circle dari posisi player
        Collider2D hit = Physics2D.OverlapCircle(transform.position, interactRange, interactMask);

        if (hit != null)
        {
            currentTarget = hit.transform;
            if (interactText != null)
                interactText.gameObject.SetActive(true);
        }
        else
        {
            currentTarget = null;
            if (interactText != null)
                interactText.gameObject.SetActive(false);
        }
    }

    void PickUpKey()
    {
        hasKey = true;
        // optional: play sound / anim
        Destroy(currentTarget.gameObject);
        currentTarget = null;
        if (interactText != null) interactText.gameObject.SetActive(false);
    }

    void TryOpenDoor()
    {
        if (hasKey)
        {
            // pintu hilang
            Destroy(currentTarget.gameObject);
            currentTarget = null;
            if (interactText != null) interactText.gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(ShowInfo("Need a Key"));
        }
    }

    IEnumerator ShowInfo(string msg)
    {
        if (infoText == null) yield break;

        infoText.text = msg;
        infoText.gameObject.SetActive(true);
        yield return new WaitForSeconds(infoDuration);
        infoText.gameObject.SetActive(false);
    }

    void UpdateInteractTextPosition()
    {
        if (currentTarget == null || interactTextRect == null || mainCam == null) return;

        // letakkan sedikit di atas posisi target (tweak offset Y sesuai sprite)
        Vector3 worldPos = currentTarget.position + Vector3.up * 0.6f;
        Vector3 screenPos = mainCam.WorldToScreenPoint(worldPos);
        interactTextRect.position = screenPos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
