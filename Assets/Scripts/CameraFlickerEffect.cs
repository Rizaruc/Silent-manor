using UnityEngine;

public class CameraFlickerEffect : MonoBehaviour
{
    public MonsterFollowSmart monster;
    public CanvasGroup flickerCanvas;

    public float flickerSpeed = 12f;
    public float flickerStrength = 0.2f;

    void Update()
    {
        if (monster == null || flickerCanvas == null)
            return;

        if (monster.IsChasing())
        {
            float flicker = Mathf.Abs(Mathf.Sin(Time.time * flickerSpeed));
            flickerCanvas.alpha = flicker * flickerStrength;
        }
        else
        {
            flickerCanvas.alpha = Mathf.Lerp(
                flickerCanvas.alpha,
                0f,
                Time.deltaTime * 6f
            );
        }
    }
}
