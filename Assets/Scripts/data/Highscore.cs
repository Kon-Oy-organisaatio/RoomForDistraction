[System.Serializable]
public class Highscore
{
    public string playerName;
    public int score;
    public int mstime;
    public string collectedItems;

    public Highscore(string playerName, int score, int mstime, string collectedItems)
    {
        this.playerName = playerName;
        this.score = score;
        this.mstime = mstime;
        this.collectedItems = collectedItems;
    }

    public override string ToString()
    {
        return $"{playerName} - Score: {score}, Time: {mstime}ms, Items: {collectedItems}";
    }
}