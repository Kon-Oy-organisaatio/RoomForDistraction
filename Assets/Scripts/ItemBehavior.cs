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
        gameObject.layer = LayerMask.NameToLayer("Interact");
    }

    public string GetUseAction()
    {
        return useAction;
    }

    public void ShowOutline()
    {
        outline.enabled = true;
    }

    public void HideOutline()
    {
        outline.enabled = false;
    }

    public void Interact()
    {
        Debug.Log("Picked up: " + itemName);
        GameManager gameManager = Helper.GetGameManager();
        gameManager.OnItemPickup(itemName);
        Destroy(gameObject);
    }

}
