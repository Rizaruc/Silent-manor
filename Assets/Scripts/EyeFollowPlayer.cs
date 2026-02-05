using UnityEngine;

public class EyeFollowPlayer : MonoBehaviour
{
    public Transform pupil;
    public Transform player;
    public float maxRadius = 0.1f;

    private Vector3 initialLocalPos;

    void Start()
    {
        initialLocalPos = pupil.localPosition;
    }

    void Update()
    {
        if (!pupil || !player) return;

        // arah dunia dari mata ke player
        Vector3 worldDir = player.position - transform.position;

        // konversi ke local space mata
        Vector3 localDir = transform.InverseTransformDirection(worldDir);

        // buang Z (2D)
        localDir.z = 0f;

        // normalisasi arah (yang penting ARAH, bukan jarak)
        localDir = localDir.normalized;

        // kalikan radius
        Vector3 offset = localDir * maxRadius;

        // apply
        pupil.localPosition = initialLocalPos + offset;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, maxRadius);
    }
}
