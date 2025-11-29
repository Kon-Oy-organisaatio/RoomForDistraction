using UnityEngine;
using System.Collections;
using Player_Script;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Tooltip("Player data scriptable object")]
    public PlayerData playerData;
    public ScoreManager scoreManager;
    public ItemManager itemManager;
    public ChecklistUI checklistUI;
    public ClockUI clockUI;
    public HighScoreManager highScoreManager;
    public PauseManager pauseManager;
    public ItemPool itemPool;

    private float gameTime = 0f; // temp
    public float gameDuration = 30f; // temp

    public Canvas gameOverCanvas;

    public void Start()
    {
        playerData.PlayerSpeedMultiplier = 1f;
        playerData.AnimationMultiplier = 1f;
        if (playerData.PlayerName == "") playerData.PlayerName = "Player";
        pauseManager.isGameOver = false;
        StartCoroutine(DelayedInit());
    }

    private IEnumerator DelayedInit()
    {
        yield return null; // wait one frame so ChecklistUI.Start() runs
        if (itemManager != null)
        {
            itemManager.InitializeItems();
        }
    }
    public void OnItemPickup(string itemName)
    {
        Debug.Log("GameManager: Item picked up - " + itemName);
        // Delegate to ItemManager
        if (itemManager != null)
        {
            itemManager.OnItemPickup(itemName);
        }

        if (itemManager.IsCorrectItem(itemName)) scoreManager.AddScore(100);
        else scoreManager.AddScore(-100);

        if (checklistUI.AllItemsCollected())
        {
            Debug.Log("GameManager: All items collected! Ending game.");
            OnGameOver();
        }
    }

    public void OnItemUse(string itemName)
    {
        if (itemName == "Coffee")
        {
            playerData.PlayerSpeedMultiplier += 0.2f;
            playerData.AnimationMultiplier -= 0.2f;
            Debug.Log("GameManager: Used Coffee. New Speed Multiplier: " + playerData.PlayerSpeedMultiplier + ", Animation Multiplier: " + playerData.AnimationMultiplier);
        }
    }

    public void OnGameOver()
    {
        pauseManager.EndGame();
        gameOverCanvas.enabled = true;
        Debug.Log("GameManager: Game Over!");
        string collectedItems = checklistUI.GetCollectedItems();
        Highscore score = new Highscore
        (
            playerData.PlayerName,
            scoreManager.GetScore(),
            (int)(gameTime * 1000),
            collectedItems
        );
        highScoreManager.AddScore(score);
        List<Highscore> highScores = HighScoreManager.LoadScores();
        highScores.Sort(new HighscoreComparer());
        Debug.Log("Highscores:");
        foreach (Highscore hs in highScores)
        {
            Debug.Log(" " + hs.ToString());
        }
        Highscore highest = highScoreManager.GetHighestScore();
        Debug.Log("Highest by " + playerData.PlayerName + " " + highest.ToString());
    }

    public void Update()
    {
        gameTime += Time.deltaTime;
        if (gameTime >= gameDuration)
        {
            OnGameOver();
            clockUI.UpdateClock(gameDuration, gameDuration);
        }
        else
        {
            clockUI.UpdateClock(gameTime, gameDuration);
        }
    }

}
