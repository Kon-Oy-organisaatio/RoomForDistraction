using UnityEngine;

public static class Bootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    static void OnGameLaunch()
    {
#if !UNITY_WEBGL
        Resolution currentResolution = Screen.currentResolution;
        float targetAspect = 1920f / 1080f;
        int maxWidth = currentResolution.width;
        int maxHeight = currentResolution.height;

        int width = maxWidth;
        int height = Mathf.RoundToInt(width / targetAspect);

        if (height > maxHeight)
        {
            height = maxHeight;
            width = Mathf.RoundToInt(height * targetAspect);
        }

        width = Mathf.RoundToInt(width * 0.8f);
        height = Mathf.RoundToInt(height * 0.8f);

        Screen.SetResolution(width, height, FullScreenMode.Windowed);
#endif
    }
}
