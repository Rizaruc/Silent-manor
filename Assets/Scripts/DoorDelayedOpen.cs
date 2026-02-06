using System.Collections;
using UnityEngine;

public class DoorDelayedOpen : MonoBehaviour
{
    [Header("Door Settings")]
    public bool needKey = true;
    public float openDelay = 0.4f;
    public float fadeSpeed = 1.5f;

    [Header("References")]
    public SpriteRenderer sprite;
    public Collider2D col;

    bool isOpening = false;

    void Awake()
    {
        if (!sprite) sprite = GetComponent<SpriteRenderer>();
        if (!col) col = GetComponent<Collider2D>();
    }

    // PANGGIL SAAT PLAYER TEKAN E
    public void Interact(bool hasKey)
    {
        if (isOpening) return;

        if (needKey && !hasKey)
        {
            // 👉 UI kamu tetap diurus script lain
            Debug.Log("Need a Key");
            return;
        }

        StartCoroutine(OpenDoorSequence());
    }

    IEnumerator OpenDoorSequence()
    {
        isOpening = true;

        // DELAY HORROR
        yield return new WaitForSeconds(openDelay);

        col.enabled = false;

        Color c = sprite.color;
        while (c.a > 0f)
        {
            c.a -= Time.deltaTime * fadeSpeed;
            sprite.color = c;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
