using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow")]
    public Transform target;              // Player
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    [Header("Shake (saat dikejar monster)")]
    public MonsterFollowSmart monster;    // drag Monster ke sini
    public float shakeStrength = 0.05f;   // seberapa brutal getarnya
    public float shakeSpeed = 20f;        // seberapa cepat getarnya

    void LateUpdate()
    {
        if (target == null)
            return;

        // ======================
        // FOLLOW NORMAL
        // ======================
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition =
            Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // ======================
        // SHAKE SAAT MONSTER NGEJAR
        // ======================
        if (monster != null && monster.IsChasing())
        {
            float x = Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f;
            float y = Mathf.PerlinNoise(0f, Time.time * shakeSpeed) - 0.5f;

            Vector3 shakeOffset = new Vector3(x, y, 0f) * shakeStrength;
            smoothedPosition += shakeOffset;
        }

        transform.position = smoothedPosition;

        // kunci rotasi
        transform.rotation = Quaternion.identity;
    }
}
