using UnityEngine;
using System.Collections.Generic;

public class HighScoreManager : MonoBehaviour
{
    public BackendHandler backendHandler;
    public List<HighScore> highScores;

    void Awake()
    {
        // Always sync from backend at startup
        SyncWithBackend();
    }

    // Called when a new score is achieved
    public void AddScore(HighScore score)
    {
        // Post directly to backend
        backendHandler.PostGameResults(score);

        // Refresh local list from backend
        SyncWithBackend();
    }

    // Merge backend scores into local list
    public void SyncWithBackend()
    {
        var scores = backendHandler.GetHighScores();
        if (scores != null && scores.scores.Length > 0)
        {
            highScores = new List<HighScore>(scores.scores);
        }
        else
        {
            highScores = new List<HighScore>();
        }
    }

    public List<HighScore> GetTopScores(int count = 5)
    {
        return highScores.GetRange(0, Mathf.Min(count, highScores.Count));
    }

    public HighScore GetHighestScore()
    {
        if (highScores.Count == 0) return null;
        return highScores[0];
    }
}
