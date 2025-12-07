using UnityEngine;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Tooltip("Player data scriptable object")]
    public PlayerData playerData;
    public ScoreManager scoreManager;
    public ItemManager itemManager;
    public ChecklistUI checklistUI;
    public ClockUI clockUI;
    public HighScoreManager highScoreManager;
    public PauseManager pauseManager;
    public ItemPool itemPool;
    public SpawnManager spawnManager;

    private float gameTime = 0f;
    public float gameDuration = 30f;
    public bool cozyMode = false;

    public Canvas gameOverCanvas;
    public TMP_Text gameOverText;

    private bool gameOverHandled = false;


    public void Start()
    {
        Instance = this;
        playerData.PlayerSpeedMultiplier = 1f;
        playerData.AnimationMultiplier = 1f;
        if (string.IsNullOrEmpty(playerData.PlayerName)) playerData.PlayerName = "Player";
        pauseManager.isGameOver = false;
        gameOverCanvas.enabled = false;
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

        if (itemManager != null)
        {
            itemManager.OnItemPickup(itemName);
        }

        if (itemManager.IsCorrectItem(itemName)) scoreManager.AddScore(100);
        else scoreManager.AddScore(-100);

        if (checklistUI.AllItemsCollected())
        {
            Debug.Log("GameManager: All items collected! Ending game.");
            gameOverText.text = "Peli päättyi\nKeräsit kaikki esineet!";
            TriggerGameOver();
        }
    }

    public void OnItemUse(string itemName)
    {
        if (itemName == "Coffee")
        {
            playerData.PlayerSpeedMultiplier += 0.2f;
            playerData.AnimationMultiplier -= 0.2f;
            Debug.Log($"GameManager: Used Coffee. New Speed Multiplier: {playerData.PlayerSpeedMultiplier}, Animation Multiplier: {playerData.AnimationMultiplier}");
        }
    }

    private void TriggerGameOver()
    {
        Debug.Log("TriggerGameOver called");
        OnGameOver();
    }

    public void OnGameOver()
    {
        if (gameOverHandled) return;
        gameOverHandled = true;

        pauseManager.EndGame();
        gameOverCanvas.enabled = true;
        Debug.Log("GameManager: Game Over!");

        string collectedItems = checklistUI.GetCollectedItems();

        HighScore score = new HighScore
        (
            playerData.PlayerName,
            scoreManager.GetScore(),
            (int)(gameTime * 1000),
            collectedItems
        );

        highScoreManager.AddScore(score);

        // Sync local cache with backend authoritative data
        highScoreManager.SyncWithBackend();

        Debug.Log($"Final Score: {score}");

        HighScore highest = highScoreManager.GetHighestScore();

        if (highest != null)
        {
            Debug.Log($"Highest score so far: {highest.playerName} with {highest.score} {highest.mstime}ms");
        }
    }


    public void Update()
    {
        if(cozyMode)
        {
            return;
        }
        if (gameOverHandled)
        {
            clockUI.UpdateClock(gameDuration, gameDuration);
            return;
        }

        gameTime += Time.deltaTime;
        if (gameTime >= gameDuration)
        {
            gameOverText.text = "Peli päättyi\nAika loppui!";
            TriggerGameOver();
            clockUI.UpdateClock(gameDuration, gameDuration);
        }
        else
        {
            clockUI.UpdateClock(gameTime, gameDuration);
        }
    }
}
