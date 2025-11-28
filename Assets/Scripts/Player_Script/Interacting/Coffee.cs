using UnityEngine;

public class Coffee : MonoBehaviour, IInteractable
{
    [Header("Outline Settings")]
    private Outline outline;
    public Color outlineColor = Color.white;
    public float outlineWidth = 5f;

    [Header("Item Settings")]
    public string useAction = "Drink";
    public string itemName = "Coffee";

    public void Start()
    {
        outline = gameObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = outlineColor;
        outline.OutlineWidth = outlineWidth;
        outline.enabled = false;
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

    public bool IsDisabled()
    {
        return false;
    }

    public void Interact()
    {
        GameManager gameManager = Helper.GetGameManager();
        gameManager.OnItemUse(itemName);
        Destroy(gameObject);
    }

    public string GetDescription()
    {
        return useAction;
    }

}
