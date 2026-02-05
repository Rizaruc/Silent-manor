using UnityEngine;

public class EyeAutoMove : MonoBehaviour
{
    public Transform[] points;     // titik tujuan
    public float moveSpeed = 1f;   // kecepatan gerak
    public float reachDistance = 0.01f;

    private int currentIndex = 0;

    void Update()
    {
        if (points == null || points.Length == 0) return;

        Transform target = points[currentIndex];

        // gerak ke point
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        // kalau sudah sampai → lanjut ke point berikutnya
        if (Vector3.Distance(transform.position, target.position) < reachDistance)
        {
            currentIndex = (currentIndex + 1) % points.Length;
        }
    }

    // biar keliatan jalurnya di Scene
    void OnDrawGizmos()
    {
        if (points == null || points.Length < 2) return;

        Gizmos.color = Color.red;
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i] == null) continue;

            Gizmos.DrawSphere(points[i].position, 0.05f);

            int next = (i + 1) % points.Length;
            if (points[next] != null)
                Gizmos.DrawLine(points[i].position, points[next].position);
        }
    }
}

