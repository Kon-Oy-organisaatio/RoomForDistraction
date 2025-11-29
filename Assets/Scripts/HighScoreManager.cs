using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class HighscoreComparer : IComparer<Highscore>
{
    public int Compare(Highscore x, Highscore y)
    {
        int nameCompare = string.Compare(x.playerName, y.playerName);
        if (nameCompare != 0)
            return nameCompare;
        if (x.score != y.score)
            return y.score.CompareTo(x.score);
        return x.mstime.CompareTo(y.mstime);
    }
}

public class HighScoreManager : MonoBehaviour
{
    public BackendAPI backendAPI;
    public List<Highscore> highScores;

    public List<Highscore>GetTopScores()
    {
        return backendAPI.GetTopscores();
    }

    public void Awake()
    {
        highScores = LoadScores();
    }

    public void AddScore(Highscore score)
    {
        highScores.Add(score);
        SaveScores(highScores);
        SendScoreToBackend(score);
    }

    public static List<Highscore> LoadScores(string filename = "highscores.json")
    {
        string path = Path.Combine(Application.persistentDataPath, filename);
        Debug.Log("Loading highscores from: " + path);
        if (!File.Exists(path)) return new List<Highscore>();
        string json = File.ReadAllText(path);
        var wrapper = JsonUtility.FromJson<HighscoreListWrapper>(json);
        return wrapper?.scores ?? new List<Highscore>();
    }

    public static void SaveScores(List<Highscore> highScores, string filename = "highscores.json")
    {
        string path = Path.Combine(Application.persistentDataPath, filename);
        Debug.Log("Saving highscores to: " + path);
        Debug.Log("Highscores to save: " + highScores.Count);
        string json = JsonUtility.ToJson(new HighscoreListWrapper { scores = highScores });
        Debug.Log("JSON: " + json);
        File.WriteAllText(path, json);
    }

    public Highscore GetHighestScore()
    {
        int highest = -1;
        int index = -1;
        for (int i = 0; i < highScores.Count; i++)
        {
            Highscore score = highScores[i];
            if (score.score > highest && score.playerName == highScores[i].playerName)
            {
                highest = score.score;
                index = i;
            }
        }
        return highScores[index];
    }

    public void SendScoreToBackend(Highscore score)
    {
        if (score.score > GetHighestScore().score)
            backendAPI.SendHighScore(score);
    }

    [System.Serializable]
    public class HighscoreListWrapper
    {
        public List<Highscore> scores;
    }
}
