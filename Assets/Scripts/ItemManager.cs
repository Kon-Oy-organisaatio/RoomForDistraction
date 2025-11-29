using UnityEngine;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    [Tooltip("Reference to the ItemPool in the scene")]
    public ItemPool itemPool;

    [Tooltip("Reference to the Checklist UI")]
    public ChecklistUI checklistUI;

    // [Tooltip("Reference to the SpawnManager")]
    // public SpawnManager spawnManager;

    private List<GameObject> targetItems = new List<GameObject>();
    private List<GameObject> distractionItems = new List<GameObject>();

    [SerializeField] private int targetCount = 5;
    [SerializeField] private int distractionCount = 5;

    /// <summary>
    /// Initializes the item sets: targets and distractions.
    /// </summary>
    public void InitializeItems()
    {
        if (itemPool == null || itemPool.itemPrefabs.Length < targetCount + distractionCount)
        {
            Debug.LogError("ItemManager: Not enough items in ItemPool. Need at least "
                           + (targetCount + distractionCount) + ", found "
                           + (itemPool == null ? 0 : itemPool.itemPrefabs.Length));
            return;
        }

        List<GameObject> availableItems = new List<GameObject>(itemPool.itemPrefabs);

        // Pick unique target items
        for (int i = 0; i < targetCount; i++)
        {
            int index = Random.Range(0, availableItems.Count);
            targetItems.Add(availableItems[index]);
            availableItems.RemoveAt(index);
        }

        // Pick unique distraction items
        for (int i = 0; i < distractionCount; i++)
        {
            int index = Random.Range(0, availableItems.Count);
            distractionItems.Add(availableItems[index]);
            availableItems.RemoveAt(index);
        }

        // Initialize ChecklistUI with targets only
        if (checklistUI != null)
        {
            InitializeChecklistUI();
        }

        // Send both sets to SpawnManager for placement
        // if (spawnManager != null)
        // {
        //     spawnManager.SpawnItems(targetItems, distractionItems);
        // }

        Debug.Log("ItemManager: Initialized with " + targetItems.Count + " targets and " + distractionItems.Count + " distractions.");
    }

    /// <summary>
    /// Adds only target items to the ChecklistUI at game start.
    /// Distractions will appear later when picked up.
    /// </summary>
    private void InitializeChecklistUI()
    {
        foreach (GameObject target in targetItems)
        {
            ItemBehavior script = target.GetComponent<ItemBehavior>();
            Debug.Log((script != null) ? script.itemName : "No ItemBehavior script found on target item.  " + target.name);
            checklistUI.items.Add(new ItemEntry(script.itemName)
            {
                isCorrect = true
            });
        }

        checklistUI.RedrawList();
    }

    /// <summary>
    /// Called by GameManager when an item is picked up.
    /// Updates ChecklistUI accordingly.
    /// </summary>
    public void OnItemPickup(string itemName)
    {
        if (checklistUI != null)
        {
            checklistUI.UpdateChecklist(itemName);
        }
    }

    public List<GameObject> GetTargetItems() => targetItems;
    public List<GameObject> GetDistractionItems() => distractionItems;
}
