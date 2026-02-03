using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    private AudioSource bgmSource;
    private float defaultVolume;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            bgmSource = GetComponent<AudioSource>();
            defaultVolume = bgmSource.volume;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Dipanggil saat jumpscare
    public void MuteBGMInstant()
    {
        if (bgmSource == null) return;
        bgmSource.volume = 0f;
    }

    // Kalau mau versi halus (opsional)
    public void FadeOutBGM(float duration)
    {
        if (bgmSource == null) return;
        StartCoroutine(FadeVolume(0f, duration));
    }

    public void RestoreBGM(float duration = 1f)
    {
        if (bgmSource == null) return;
        StartCoroutine(FadeVolume(defaultVolume, duration));
    }

    private IEnumerator FadeVolume(float targetVolume, float duration)
    {
        float startVolume = bgmSource.volume;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, targetVolume, t / duration);
            yield return null;
        }

        bgmSource.volume = targetVolume;
    }
}
