using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Tooltip("Player data scriptable object")]
    public PlayerData playerData;
    public ScoreManager scoreManager;
    public ItemManager itemManager;
    public ChecklistUI checklistUI;
    public ClockUI clockUI;
    //public SceneLoader sceneLoader;
    //public HighscoreManager highscoreManager;
    public PauseManager pauseManager;
    public ItemPool itemPool;

    private float gameTime = 0f; // temp
    public float gameDuration = 30f; // temp

    // täällä vai PauseManagerissa?
    public Canvas gameOverCanvas;

    public void Start()
    {
        playerData.PlayerSpeedMultiplier = 1f;
        playerData.AnimationMultiplier = 1f;
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
        //pauseManager.EndGame();
        //gameOverCanvas.enabled = true;
        clockUI.UpdateClock(gameDuration, gameDuration);
        Debug.Log("GameManager: Game Over!");
        string collectedItems = checklistUI.GetCollectedItems();
        //highscoreManager.AddScore(playerData.name, scoreManager.GetScore(), gameTime, collectedItems);
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
