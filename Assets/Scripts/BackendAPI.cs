using UnityEngine;
using System.Collections.Generic;

public class BackendAPI : MonoBehaviour
{
    
    public List<Highscore> GetTopscores()
    {
        // Placeholder
        return new List<Highscore>
        {
            // name, score, mstime, collectedItems
            new("Player1", 500, 28000, "ItemA, ItemB"),
            new("Player2", 400, 400, "ItemC, ItemD")
        };
    }

    public void SendHighScore(Highscore score)
    {}

}
