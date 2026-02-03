using UnityEngine;

public class MonsterChase : MonoBehaviour
{
    public Transform player;
    public float chaseDistance = 5f;
    public float speed = 2f;

    AudioSource screamAudio;
    bool isChasing = false;

    void Start()
    {
        screamAudio = GetComponent<AudioSource>();
        screamAudio.loop = true;          // biar ambience ngejar
        screamAudio.playOnAwake = false;
        screamAudio.volume = 0f;          // mulai dari senyap
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= chaseDistance)
        {
            ChasePlayer(distance);
        }
        else
        {
            StopChase();
        }
    }

    void ChasePlayer(float distance)
    {
        // gerak ke player
        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );

        // play sound cuma SEKALI pas mulai ngejar
        if (!isChasing)
        {
            isChasing = true;
            screamAudio.Play();
        }

        // volume makin keras saat makin dekat
        float targetVolume = 1f - (distance / chaseDistance);
        screamAudio.volume = Mathf.Lerp(
            screamAudio.volume,
            targetVolume,
            Time.deltaTime * 3f   // smooth speed
        );
    }

    void StopChase()
    {
        if (isChasing)
        {
            isChasing = false;
            screamAudio.Stop();
            screamAudio.volume = 0f;
        }
    }
}
