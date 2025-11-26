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
        GameManager gameManager = Helper.GetGameManager();
        gameManager.OnItemUse(itemName);
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

}
