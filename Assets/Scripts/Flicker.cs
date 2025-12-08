using UnityEngine;

public class Flicker : MonoBehaviour
{
    private Light lamp;
    private float baseIntensity;
    private float baseRange;
    private float flickerTime;

    void Start()
    {
        lamp = GetComponentInChildren<Light>();
        if (lamp != null && lamp.type == LightType.Point)
        {
            baseIntensity = lamp.intensity;
            baseRange = lamp.range;
        }
        flickerTime = Random.Range(0f, 100f);
    }

    void Update()
    {
        if (lamp == null || lamp.type != LightType.Point) return;

        flickerTime += Time.deltaTime * 2.5f;
        float flicker = Mathf.PerlinNoise(flickerTime, 0f);
        lamp.intensity = baseIntensity * (0.7f + flicker * 0.6f);
        lamp.range = baseRange + Mathf.Lerp(-0.2f * baseRange, 0.2f * baseRange, flicker);
    }
}
