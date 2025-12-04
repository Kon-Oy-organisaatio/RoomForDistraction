using UnityEngine;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    [Tooltip("Reference to the ItemPool in the scene")]
    public ItemPool itemPool;

    [Tooltip("Reference to the Checklist UI")]
    public ChecklistUI checklistUI;

    [Tooltip("Reference to the SpawnManager")]
    public SpawnManager spawnManager;

    private List<GameObject> targetItems = new List<GameObject>();
    private List<GameObject> distractionItems = new List<GameObject>();

    [SerializeField] private int targetCount = 5;
    [SerializeField] private int distractionCount = 5;

    /// <summary>
    /// Initializes the item sets: targets and distractions.
    /// </summary>
    public void InitializeItems()
    {
        targetItems.Clear();
        distractionItems.Clear();

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
        if (spawnManager != null)
        {
            spawnManager.SpawnItems(targetItems, distractionItems);
        }

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
            if (script == null)
            {
                Debug.LogError("ItemManager: Target prefab missing ItemBehavior: " + target.name);
                continue;
            }

            // ChecklistUI only has AddItem(string)
            checklistUI.AddItem(script.itemName);
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
            // ChecklistUI only has UpdateChecklist(string)
            checklistUI.UpdateChecklist(itemName);
        }
    }

    /// <summary>
    /// Checks if the given item name is in the target items list.
    /// </summary>
    public bool IsCorrectItem(string itemName)
    {
        foreach (GameObject target in targetItems)
        {
            ItemBehavior script = target.GetComponent<ItemBehavior>();
            if (script != null && script.itemName == itemName)
            {
                return true;
            }
        }
        return false;
    }

    public List<GameObject> GetTargetItems() => targetItems;
    public List<GameObject> GetDistractionItems() => distractionItems;
}
