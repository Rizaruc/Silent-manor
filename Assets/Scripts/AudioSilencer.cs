using UnityEngine;

public class AudioSilencer : MonoBehaviour
{
    public static AudioSilencer instance;

    AudioSource[] cachedSources;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // 🔇 Matikan semua audio yang lagi main
    public void SilenceAll()
    {
        cachedSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource src in cachedSources)
        {
            if (src != null && src.isPlaying)
                src.Pause();
        }
    }

    // 🔊 Hidupkan lagi semua audio
    public void ResumeAll()
    {
        if (cachedSources == null) return;

        foreach (AudioSource src in cachedSources)
        {
            if (src != null)
                src.UnPause();
        }
    }
}
