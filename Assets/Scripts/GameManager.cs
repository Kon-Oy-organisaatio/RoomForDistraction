using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Tooltip("Player data scriptable object")]
    public PlayerData playerData;

    public void OnItemPickup(string itemName)
    {
        Debug.Log("GameManager: Item picked up - " + itemName);
    }
}
