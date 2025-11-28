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


    public void Start()
    {
        if (Notelist == null)
        {
            Notelist = GetComponent<Canvas>();
        }
        items = new List<ItemEntry> {};
        textElements = Notelist.transform.Find("Items").GetComponentsInChildren<TMP_Text>();
        imageElements = Notelist.transform.Find("Strikes").GetComponentsInChildren<Image>();
        for (int i = 0; i < textElements.Length; i++)
        {
            textElements[i].text = "";
            textElements[i].color = defaultColor;
            imageElements[i].enabled = false;
        }
        #if UNITY_EDITOR
            StartCoroutine(UpdateChecklist());
        #endif
    }

    #if UNITY_EDITOR
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
                if (item.isCorrect)
                {
                    textElement.color = correctColor;
                }
                else
                {
                    textElement.color = incorrectColor;
                }
                if (item.isChecked)
                {
                    imageElement.enabled = true;
                }
                else
                {
                    imageElement.enabled = false;
                }
            }
            index++;
        }
    }
}
