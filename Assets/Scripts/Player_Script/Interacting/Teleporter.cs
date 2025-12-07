using UnityEngine;

public class Teleporter : MonoBehaviour, IInteractable
{
    [Header("Outline Settings")]
    public Outline outline;
    public Color outlineColor = Color.white;
    public float outlineWidth = 5f;

    [Header("Teleport Settings")]
    public string useAction = "???";
    public Vector3 teleportLocation;
    public float ambientLightIntensity = 1f;


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
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            while (player.transform.parent != null && player.transform.parent.tag == "Player")
            {
                player = player.transform.parent.gameObject;
            }
            var controller = player.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false;
                player.transform.position = teleportLocation;
                controller.enabled = true;
            }
        }
        RenderSettings.ambientIntensity = ambientLightIntensity;
    }

    public string GetDescription()
    {
        return useAction;
    }

}
