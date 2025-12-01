using UnityEngine;
using System.Collections;

public class InteractObject : MonoBehaviour, IInteractable
{
    [Header("Outline Settings")]
    [Tooltip("Object to outline (if null, uses self)")]
    public GameObject objectToOutline = null;
    [Tooltip("Outline color")]
    public Color outlineColor = Color.white;
    [Tooltip("Outline width")]
    public float outlineWidth = 5f;
    private Outline outline;

    [Header("Interaction Settings")]
    [Tooltip("Object to animate (if null, uses self)")]
    public GameObject objectToAnimate = null;
    [Tooltip("Relative position to move when opened")]
    public Vector3 relativePosition;
    [Tooltip("Relative rotation to apply when opened")]
    public Vector3 relativeRotation;
    [Tooltip("Scale to apply when opened")]
    public Vector3 targetScale;
    [Tooltip("Duration of the open/close animation in seconds")]
    public float animationDuration = 0.5f;
    [Tooltip("Action text for use, format: 'Open/Close'")]
    public string useAction = "Open/Close";
    [Tooltip("Object to activate/deactivate on interaction")]
    public GameObject activationTarget;
    [Tooltip("If true disables interaction after animation")]
    public bool disableInteractionAfterAnimation = false;
    [Tooltip("Audio to play on open")]
    public AudioClip openAudio;
    [Tooltip("Audio to play on close")]
    public AudioClip closeAudio;
    private bool state = false;
    private Vector3 initialScale;
    public bool disabled = false;
    
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 startScale;
    private Coroutine currentAnimation;

    public void Start()
    {
        if (objectToOutline == null) objectToOutline = gameObject;
        if (objectToAnimate == null) objectToAnimate = gameObject;
        outline = objectToOutline.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = outlineColor;
        outline.OutlineWidth = outlineWidth;
        outline.enabled = false;
        startPosition = objectToAnimate.transform.localPosition;
        startRotation = objectToAnimate.transform.localRotation;
        startScale = objectToAnimate.transform.localScale;
        initialScale = objectToAnimate.transform.localScale;
        for (int i = 0; i < 3; i++)
        {
            if ( targetScale[i] == 0)
            {
                targetScale[i] = initialScale[i];
            }
                
        }
    }

    public bool IsDisabled()
    {
        return disabled;
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
        if (currentAnimation != null || disabled)
            return;
        state = !state;
        currentAnimation = StartCoroutine(Animate(state));
    }

    public string GetDescription()
    {
        return useAction.Split('/')[state ? 1 : 0].Trim();
    }

    private IEnumerator Animate(bool open)
    {
        if (state && openAudio != null)
        {
            AudioSource.PlayClipAtPoint(openAudio, objectToAnimate.transform.position);
        }
        else if (!state && closeAudio != null)
        {
            AudioSource.PlayClipAtPoint(closeAudio, objectToAnimate.transform.position);
        }

        Vector3 targetPosition = open ? startPosition + relativePosition : startPosition;
        Quaternion targetRotation = open ? Quaternion.Euler(relativeRotation) * startRotation : startRotation;
        Vector3 scale =  open ? targetScale : startScale;

        Vector3 initialPosition = objectToAnimate.transform.localPosition;
        Quaternion initialRotation = objectToAnimate.transform.localRotation;
        Vector3 initialScale = objectToAnimate.transform.localScale;

        float elapsed = 0f;
        float duration = animationDuration / GameManager.Instance.playerData.AnimationMultiplier;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            objectToAnimate.transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, t);
            objectToAnimate.transform.localRotation = Quaternion.Slerp(initialRotation, targetRotation, t);
            for (int i = 0; i < 3; i++)
            {
                float scaleValue = Mathf.Lerp(initialScale[i], scale[i], t);
                Vector3 currentScale = objectToAnimate.transform.localScale;
                currentScale[i] = scaleValue;
                objectToAnimate.transform.localScale = currentScale;
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
        if (activationTarget != null)
        {
            activationTarget.SetActive(open);
        }
        if (disableInteractionAfterAnimation)
        {
            disabled = true;
        }

        objectToAnimate.transform.localPosition = targetPosition;
        objectToAnimate.transform.localRotation = targetRotation;
        objectToAnimate.transform.localScale = scale;

        currentAnimation = null;
    }
}
