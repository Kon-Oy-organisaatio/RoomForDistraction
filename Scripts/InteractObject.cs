using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class InteractObject : MonoBehaviour
{
    private Outline outline;
    public Color outlineColor = Color.white;
    public float outlineWidth = 5f;

    private bool isOpen = false;
    public Vector3 relativePosition;
    public Vector3 relativeRotation;
    public float animationDuration = 0.5f;
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

    public void ShowOutline()
    {
        outline.enabled = true;
    }

    public void HideOutline()
    {
        outline.enabled = false;
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

    private IEnumerator AnimateDrawer(bool open)
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

        transform.localPosition = targetPosition;
        transform.localRotation = targetRotation;
    }

    public void Interact()
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }
        isOpen = !isOpen;
        currentAnimation = StartCoroutine(AnimateDrawer(isOpen));
    }

}
