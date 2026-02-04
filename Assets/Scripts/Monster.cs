using UnityEngine;

public class MonsterFollowSmart : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Chase Settings")]
    public float chaseStartRadius = 6f;
    public float chaseStopRadius = 8f;
    public float moveSpeed = 2f;

    private bool isChasing = false;
    private bool isFacingRight = true;

    void Update()
    {
        if (player == null)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Mulai mengejar
        if (!isChasing && distance <= chaseStartRadius)
        {
            isChasing = true;
        }
        // Berhenti mengejar
        else if (isChasing && distance > chaseStopRadius)
        {
            isChasing = false;
        }

        if (isChasing)
            FollowPlayer();
    }

    private void FollowPlayer()
    {
        // Gerak ke player (X saja)
        transform.position = Vector2.MoveTowards(
            transform.position,
            new Vector2(player.position.x, transform.position.y),
            moveSpeed * Time.deltaTime
        );

        // Flip sprite
        if (player.position.x > transform.position.x && !isFacingRight)
            Flip();
        else if (player.position.x < transform.position.x && isFacingRight)
            Flip();
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // =========================
    // DIPAKAI SCRIPT LAIN
    // =========================
    public bool IsChasing()
    {
        return isChasing;
    }

    // =========================
    // GIZMOS
    // =========================
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseStartRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseStopRadius);
    }
}
