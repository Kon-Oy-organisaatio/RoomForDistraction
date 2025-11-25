using UnityEngine;


public static class Helper
{
    /// <summary>
    /// Returns the first active GameManager in the scene.
    /// </summary>
    public static GameManager GetGameManager()
    {
        return Object.FindFirstObjectByType<GameManager>();
    }
}