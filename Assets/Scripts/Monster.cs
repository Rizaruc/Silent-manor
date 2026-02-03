using UnityEngine;

public class MonsterFollowSmart : MonoBehaviour
{
    public Transform player;          // Target yang diikuti (Player)
    public float chaseStartRadius = 6f;  // Jarak mulai mengejar
    public float chaseStopRadius = 8f;   // Jarak berhenti mengejar
    public float moveSpeed = 2f;         // Kecepatan monster

    private bool isChasing = false;
    private bool isFacingRight = true;

    void Update()
    {
        if (player == null)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Mulai mengejar saat player masuk radius
        if (!isChasing && distance <= chaseStartRadius)
        {
            isChasing = true;
        }
        // Berhenti mengejar saat player menjauh lebih jauh dari batas
        else if (isChasing && distance > chaseStopRadius)
        {
            isChasing = false;
        }

        if (isChasing)
        {
            FollowPlayer();
        }
        else
        {
            StopMoving();
        }
    }

    private void FollowPlayer()
    {
        // Gerak mendekati player (sumbu X saja)
        transform.position = Vector2.MoveTowards(
            transform.position,
            new Vector2(player.position.x, transform.position.y),
            moveSpeed * Time.deltaTime
        );

        // Balik arah jika perlu
        if (player.position.x > transform.position.x && !isFacingRight)
        {
            Flip();
        }
        else if (player.position.x < transform.position.x && isFacingRight)
        {
            Flip();
        }
    }

    private void StopMoving()
    {
        // Bisa ditambah animasi Idle di sini kalau mau
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Biar kelihatan radius di Scene View
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseStartRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseStopRadius);
    }
}
