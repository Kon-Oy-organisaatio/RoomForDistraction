using UnityEngine;

[ExecuteAlways]
public class SetSkyFromLight : MonoBehaviour
{
    public Light directionalLight;
    [Range(0f, 8f)] public float exposure = 1f;
    [Range(0.001f, 0.2f)] public float sunSize = 0.04f;
    [Range(1f, 32f)] public float sunConvergence = 8f;
    public Color sunColor = Color.white;

    // Optional: let the script drive the cubemap blend so you can reveal the procedural sun
    public bool overrideCubemapBlend = false;
    [Range(0f, 1f)] public float cubemapBlend = 0.5f;

    void Update()
    {
        if (directionalLight != null)
        {
            // directional lights: direction is -forward in world space
            Vector3 sunDir = -directionalLight.transform.forward;
            sunDir.Normalize(); // ensure normalized direction for the shader
            Shader.SetGlobalVector("_SunDirection",
                                   new Vector4(sunDir.x, sunDir.y, sunDir.z, 0));

            // Use inspector sunColor multiplier so you can tint/boost sun independently.
            Color finalSunColor = directionalLight.color * directionalLight.intensity;
            finalSunColor *= sunColor;
            Shader.SetGlobalColor("_SunColor", finalSunColor);
        }

        // Optionally override the material's cubemap blend from the inspector.
        if (overrideCubemapBlend)
        {
            Shader.SetGlobalFloat("_CubemapBlend", Mathf.Clamp01(cubemapBlend));
        }

        Shader.SetGlobalFloat("_Exposure", exposure);
        Shader.SetGlobalFloat("_SunSize", sunSize);
        Shader.SetGlobalFloat("_SunConvergence", sunConvergence);
    }
}
