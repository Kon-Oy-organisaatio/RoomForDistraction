using UnityEngine;

public class StaggeredLightColorCycle : MonoBehaviour
{
    [Header("Color Cycle Settings")]
    [Tooltip("How long it takes to complete one full color cycle (seconds).")]
    public float cycleDuration = 3f;

    [Tooltip("How much delay (phase offset) between each light's cycle (0–1).")]
    public float staggerAmount = 0.2f;

    private Light[] lights;
    private float startTime;

    void Start()
    {
        startTime = Time.time;
        lights = GetComponentsInChildren<Light>();
    }

    void Update()
    {
        if (lights == null || lights.Length == 0)
            return;

        float t = (Time.time - startTime) / cycleDuration;

        for (int i = 0; i < lights.Length; i++)
        {
            float hue = Mathf.Repeat(t + i * staggerAmount, 1f);
            lights[i].color = Color.HSVToRGB(hue, 1f, 1f);
        }
    }
}
