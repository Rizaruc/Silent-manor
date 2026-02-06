using UnityEngine;

public class EyeFollowPlayer : MonoBehaviour
{
    public Transform pupil;
    public Transform player;

    [Header("Batas Gerak Pupil")]
    public float maxX = 0.12f;
    public float maxY = 0.08f;

    [Header("Kehalusan Gerakan")]
    public float smoothSpeed = 8f; // makin kecil = makin lambat

    private Vector3 initialLocalPos;
    private Vector3 currentVelocity;

    void Start()
    {
        initialLocalPos = pupil.localPosition;
    }

    void Update()
    {
        if (!pupil || !player) return;

        // posisi player di local space mata
        Vector3 localTarget = transform.InverseTransformPoint(player.position);

        // offset relatif dari posisi awal pupil
        Vector3 offset = localTarget - initialLocalPos;

        // clamp agar tetap di dalam bola mata
        offset.x = Mathf.Clamp(offset.x, -maxX, maxX);
        offset.y = Mathf.Clamp(offset.y, -maxY, maxY);
        offset.z = 0f;

        Vector3 targetPos = initialLocalPos + offset;

        // SMOOTH FOLLOW
        pupil.localPosition = Vector3.Lerp(
            pupil.localPosition,
            targetPos,
            Time.deltaTime * smoothSpeed
        );
    }
}
