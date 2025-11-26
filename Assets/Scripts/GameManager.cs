using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Tooltip("Player data scriptable object")]
    public PlayerData playerData;

    public void Start()
    {
        playerData.PlayerSpeedMultiplier = 1f;
        playerData.AnimationMultiplier = 1f;
    }

    public void OnItemPickup(string itemName)
    {
        Debug.Log("GameManager: Item picked up - " + itemName);
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

}
