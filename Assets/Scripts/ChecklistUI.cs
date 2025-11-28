using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class ItemEntry
{
    public string itemName;
    public bool isChecked;
    public bool isCorrect;

    public ItemEntry(string name)
    {
        itemName = name;
        isChecked = false;
        isCorrect = false;
    }
}

public class ChecklistUI : MonoBehaviour
{
    // public for testing purposes
    public List<ItemEntry> items;
    public Canvas Notelist;
    public Color defaultColor;
    public Color correctColor;
    public Color incorrectColor;

    private TMP_Text[] textElements;
    private Image[] imageElements;

    public void Awake()
    {
        if (Notelist == null)
        {
            Notelist = GetComponent<Canvas>();
        }

        items = new List<ItemEntry>();

        // Cache UI references
        Transform itemsRoot = Notelist.transform.Find("Items");
        Transform strikesRoot = Notelist.transform.Find("Strikes");

        if (itemsRoot != null)
        {
            textElements = itemsRoot.GetComponentsInChildren<TMP_Text>();
        }
        if (strikesRoot != null)
        {
            imageElements = strikesRoot.GetComponentsInChildren<Image>();
        }

        // Initialize UI elements
        if (textElements != null && imageElements != null)
        {
            for (int i = 0; i < textElements.Length; i++)
            {
                textElements[i].text = "";
                textElements[i].color = defaultColor;
                if (i < imageElements.Length)
                {
                    imageElements[i].enabled = false;
                }
            }
        }
    }

#if UNITY_EDITOR
    public void Start()
    {
        // Editor-only auto-refresh for debugging
        StartCoroutine(UpdateChecklist());
    }

    IEnumerator UpdateChecklist()
    {
        while (true)
        {
            RedrawList();
            yield return new WaitForSeconds(1f);
        }
    }
#endif

    public void AddItem(string itemName)
    {
        items.Add(new ItemEntry(itemName));
        RedrawList();
    }

    public void UpdateChecklist(string itemName)
    {
        foreach (ItemEntry item in items)
        {
            if (item.itemName == itemName)
            {
                item.isChecked = true;
                item.isCorrect = true;
                return;
            }
        }
        // If not found, add as incorrect
        items.Add(new ItemEntry(itemName) { isChecked = true, isCorrect = false });
    }

    public string GetCollectedItems()
    {
        List<string> collected = new List<string>();
        foreach (ItemEntry item in items)
        {
            if (item.isChecked)
            {
                collected.Add(item.itemName);
            }
        }
        return string.Join(", ", collected);
    }

    public void RedrawList()
    {
        if (textElements == null || imageElements == null)
        {
            Debug.LogWarning("ChecklistUI: UI elements not initialized yet.");
            return;
        }

        int index = 0;
        foreach (ItemEntry item in items)
        {
            if (index >= textElements.Length)
                break;

            TMP_Text textElement = textElements[index];
            Image imageElement = imageElements[index];

            textElement.text = item.itemName;

            if (item.isChecked)
            {
                textElement.color = item.isCorrect ? correctColor : incorrectColor;
                imageElement.enabled = true;
            }
            else
            {
                textElement.color = defaultColor;
                imageElement.enabled = false;
            }

            index++;
        }
    }
}
