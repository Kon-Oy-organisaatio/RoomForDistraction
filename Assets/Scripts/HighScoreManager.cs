using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class HighScoreManager : MonoBehaviour
{
    public BackendHandler backendHandler;
    public List<HighScore> highScores;

    void Awake()
    {
        highScores = LoadScores();
    }

    // Called when a new score is achieved
    public void AddScore(HighScore score)
    {
        highScores.Add(score);
        SaveScores(highScores);
        backendHandler.PostGameResults(score);
    }

    // Merge backend scores into local cache
    public void SyncWithBackend()
    {
        var scores = backendHandler.GetHighScores();
        if (scores != null && scores.scores.Length > 0)
        {
            highScores = new List<HighScore>(scores.scores);
            SaveScores(highScores);
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

    public static List<HighScore> LoadScores(string filename = "highscores.json")
    {
        string path = Path.Combine(Application.persistentDataPath, filename);
        Debug.Log("Loading highscores from: " + path);

        if (!File.Exists(path)) return new List<HighScore>();

        string json = File.ReadAllText(path);
        var wrapper = JsonUtility.FromJson<HighScoreListWrapper>(json);
        return wrapper?.scores ?? new List<HighScore>();
    }

    public static void SaveScores(List<HighScore> highScores, string filename = "highscores.json")
    {
        string path = Path.Combine(Application.persistentDataPath, filename);
        Debug.Log("Saving highscores to: " + path);

        string json = JsonUtility.ToJson(new HighScoreListWrapper { scores = highScores });
        File.WriteAllText(path, json);
    }

    [System.Serializable]
    public class HighScoreListWrapper
    {
        public List<HighScore> scores;
    }
}
