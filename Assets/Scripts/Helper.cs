using UnityEngine;


public static class Helper
{
    public static GameManager GetGameManager()
    {
        return Object.FindFirstObjectByType<GameManager>();
    }
}