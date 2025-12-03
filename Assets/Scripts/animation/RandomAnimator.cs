using UnityEngine;

public class RandomAnimator : MonoBehaviour
{
    private RandomAnimationParams animationParams;

    private Vector3 originalPosition;
    private Vector3 initialPosition;
    private Vector3 targetPosition;

    private Vector3 originalScale;
    private Vector3 initialScale;
    private Vector3 targetScale;

    private float initialOutlineWidth;
    private float targetOutlineWidth;

    private Color initialOutlineColor;
    private Color targetOutlineColor;
    
    private float moveSpeed;
    private float lerpTime = 0f;
    private float lerpDuration = 1f;

    private ItemBehavior itemBehavior;

    private Vector3 rotationDirection;
    private Vector3 currentRotation;

    public void Init(RandomAnimationParams animationParams)
    {
        this.animationParams = animationParams;

        originalPosition = transform.position;
        originalScale = transform.localScale;

        rotationDirection = new Vector3(
            Random.Range(-animationParams.rotationRange, animationParams.rotationRange),
            Random.Range(-animationParams.rotationRange, animationParams.rotationRange),
            Random.Range(-animationParams.rotationRange, animationParams.rotationRange)
        );
        currentRotation = transform.rotation.eulerAngles;

        if (TryGetComponent(out itemBehavior) && itemBehavior != null)
        {
            itemBehavior.outline.OutlineColor = new Color(Random.value, Random.value, Random.value);
            itemBehavior.outline.OutlineWidth = Random.Range(1f, 10f);
            itemBehavior.ShowOutline();
        }

        PickNewTarget();
    }

    private void PickNewTarget()
    {
        initialPosition = transform.position;
        initialScale = transform.localScale;
        if (itemBehavior != null && itemBehavior.outline != null)
        {
            initialOutlineWidth = itemBehavior.outline.OutlineWidth;
            initialOutlineColor = itemBehavior.outline.OutlineColor;
        }

        targetPosition = originalPosition + new Vector3(
            Random.Range(-animationParams.positionRange, animationParams.positionRange),
            0f,
            Random.Range(-animationParams.positionRange, animationParams.positionRange)
        );
        float scaleFactor = Random.Range(animationParams.scaleMin, animationParams.scaleMax);
        targetScale = originalScale * scaleFactor;
        targetOutlineWidth = Random.Range(1f, 10f);
        targetOutlineColor = new Color(Random.value, Random.value, Random.value);

        moveSpeed = Random.Range(animationParams.speedMin, animationParams.speedMax);
        lerpTime = 0f;
        lerpDuration = Vector3.Distance(initialPosition, targetPosition) / moveSpeed;
    }

    // EaseInOut function for smooth movement
    private float EaseInOut(float t)
    {
        return t < 0.5f
            ? 2f * t * t
            : -1f + (4f - 2f * t) * t;
    }

    private void Animate(Vector3 fromPos, Vector3 toPos, Vector3 fromScale, Vector3 toScale,
        float fromOutline, float toOutline,
        Color fromColor, Color toColor, float t, float easedT)
    {
        transform.position = Vector3.Lerp(fromPos, toPos, easedT);
        transform.localScale = Vector3.Lerp(fromScale, toScale, t);
        if (itemBehavior != null && itemBehavior.outline != null)
        {
            itemBehavior.outline.OutlineWidth = Mathf.Lerp(fromOutline, toOutline, t);
            itemBehavior.outline.OutlineColor = Color.Lerp(fromColor, toColor, t);
        }
    }

    void Update()
    {
        if (initialPosition == Vector3.zero) return; // Not initialized yet

        lerpTime += Time.deltaTime;
        float t = Mathf.Clamp01(lerpTime / lerpDuration);
        float easedT = EaseInOut(t);

        Animate(initialPosition, targetPosition, initialScale, targetScale,
            initialOutlineWidth, targetOutlineWidth,
            initialOutlineColor, targetOutlineColor, t, easedT);

        currentRotation += rotationDirection * Time.deltaTime * animationParams.rotationSpeed;
        transform.rotation = Quaternion.Euler(currentRotation);

        if (t >= 1f)
        {
            PickNewTarget();
        }
    }
}