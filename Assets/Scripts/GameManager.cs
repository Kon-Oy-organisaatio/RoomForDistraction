using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public void OnItemPickup(string itemName)
    {
        Debug.Log("GameManager: Item picked up - " + itemName);
    }
}
