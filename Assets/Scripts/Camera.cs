using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Player yang akan diikuti
    public float smoothSpeed = 0.125f; // Semakin kecil nilainya, semakin halus
    public Vector3 offset; // Jarak antara kamera dan player

    void LateUpdate()
    {
        if (target == null)
            return;

        // Posisi kamera yang diinginkan
        Vector3 desiredPosition = target.position + offset;

        // Gerakan halus (lerp)
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;

        // Kunci rotasi kamera agar tidak miring
        transform.rotation = Quaternion.identity;
    }
}
