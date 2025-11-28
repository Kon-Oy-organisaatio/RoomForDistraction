using UnityEngine;

public class ItemBehavior : MonoBehaviour, IInteractable
{
    [Header("Outline Settings")]
    private Outline outline;
    public Color outlineColor = Color.white;
    public float outlineWidth = 5f;

    [Header("Item Settings")]
    public string useAction = "Pickup Item";
    public string itemName = "Item";

    public void Start()
    {
        outline = gameObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = outlineColor;
        outline.OutlineWidth = outlineWidth;
        outline.enabled = false;
    }

    public bool IsDisabled()
    {
        return false;
    }

    public void ShowOutline()
    {
        if (outline != null)
            outline.enabled = true;
    }

    public void HideOutline()
    {
        if (outline != null)
            outline.enabled = false;
    }

    public void Interact()
    {
        Debug.Log("Picked up: " + itemName);
        GameManager gameManager = Helper.GetGameManager();
        gameManager.OnItemPickup(itemName);
        Destroy(gameObject);
    }

    public string GetDescription()
    {
        return useAction;
    }

}
