using UnityEngine;

public class ItemBehavior : MonoBehaviour, IInteractable
{
    [Header("Outline Settings")]
    public Outline outline;
    public Color outlineColor = Color.white;
    public float outlineWidth = 5f;

    [Header("Item Settings")]
    public string useAction = "Poimi ";
    public string itemName = "Tavara";
    public string lookAction = "Katso, ";

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
        if (!GameManager.Instance.cozyMode)
        {
            Debug.Log("Picked up: " + itemName);
            GameManager.Instance.OnItemPickup(itemName);
            Destroy(gameObject);
        }
    }

    public string GetDescription()
    {
        if (!GameManager.Instance.cozyMode) return useAction + itemName;
        return lookAction + itemName;
    }

}
