[System.Serializable]
public class HighScore
{
    public string playerName;
    public int score;
    public int mstime;
    public string collectedItems;

    // Optional parameterless constructor
    public HighScore() {}

    public HighScore(string playerName, int score, int mstime, string collectedItems)
    {
        this.playerName = playerName;
        this.score = score;
        this.mstime = mstime;
        this.collectedItems = collectedItems;
    }
}

[System.Serializable]
public class HighScoreList
{
    public HighScore[] scores;
}
