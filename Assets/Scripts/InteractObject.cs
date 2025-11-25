using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class InteractObject : MonoBehaviour, IInteractable
{
    private Outline outline;
    public Color outlineColor = Color.white;
    public float outlineWidth = 5f;

    private bool state = false;
    [Tooltip("Relative position to move when opened")]
    public Vector3 relativePosition;
    [Tooltip("Relative rotation to apply when opened")]
    public Vector3 relativeRotation;
    [Tooltip("Duration of the open/close animation in seconds")]
    public float animationDuration = 0.5f;
    [Tooltip("Action text for use, format: 'Open/Close'")]
    public string useAction = "Open/Close";
    [Tooltip("Object to activate/deactivate on interaction")]
    public GameObject activationTarget;
    
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Coroutine currentAnimation;

    public void Start()
    {
        outline = gameObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = outlineColor;
        outline.OutlineWidth = outlineWidth;
        outline.enabled = false;
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
    }

    public string GetUseAction()
    {
        return useAction.Split('/')[state ? 1 : 0].Trim();
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
        if (currentAnimation != null)
            return;
        state = !state;
        currentAnimation = StartCoroutine(Animate(state));
    }

    private IEnumerator Animate(bool open)
    {
        Vector3 targetPosition = open ? startPosition + relativePosition : startPosition;
        Quaternion targetRotation = open ? Quaternion.Euler(relativeRotation) * startRotation : startRotation;

        Vector3 initialPosition = transform.localPosition;
        Quaternion initialRotation = transform.localRotation;

        float elapsed = 0f;
        while (elapsed < animationDuration)
        {
            float t = elapsed / animationDuration;
            transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, t);
            transform.localRotation = Quaternion.Slerp(initialRotation, targetRotation, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        if (activationTarget != null)
        {
            activationTarget.SetActive(open);
        }

        transform.localPosition = targetPosition;
        transform.localRotation = targetRotation;
        currentAnimation = null;
    }


#if UNITY_EDITOR
    public void Update()
    {
        if (Keyboard.current != null)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                Interact();
            }
            if (Keyboard.current.fKey.isPressed)
            {
                ShowOutline();
            }
            else
            {
                HideOutline();
            }
            return;
        }
    }
#endif
}
