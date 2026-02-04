using UnityEngine;

public class MonsterChase : MonoBehaviour
{
    public Transform player;
    public float chaseDistance = 5f;
    public float speed = 2f;

    [Header("Audio")]
    public float fadeSpeed = 2f; // makin besar = makin cepat hilang

    AudioSource screamAudio;
    bool isChasing = false;

    void Start()
    {
        screamAudio = GetComponent<AudioSource>();
        screamAudio.loop = true;
        screamAudio.playOnAwake = false;
        screamAudio.volume = 0f;
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
            FadeOutSound();
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

        if (!isChasing)
        {
            isChasing = true;
            screamAudio.Play();
        }

        // volume makin dekat makin keras
        float targetVolume = 1f - (distance / chaseDistance);
        screamAudio.volume = Mathf.Lerp(
            screamAudio.volume,
            targetVolume,
            Time.deltaTime * 4f
        );
    }

    void FadeOutSound()
    {
        if (!isChasing) return;

        // turunin volume pelan
        screamAudio.volume = Mathf.Lerp(
            screamAudio.volume,
            0f,
            Time.deltaTime * fadeSpeed
        );

        // kalo udah hampir senyap → stop
        if (screamAudio.volume <= 0.01f)
        {
            screamAudio.Stop();
            screamAudio.volume = 0f;
            isChasing = false;
        }
    }
}
