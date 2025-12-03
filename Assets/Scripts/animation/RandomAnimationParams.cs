using UnityEngine;

[CreateAssetMenu(fileName = "RandomAnimationParams", menuName = "Scriptable Objects/RandomAnimationParams")]
public class RandomAnimationParams : ScriptableObject
{
    public float positionRange = 0.5f;
    public float scaleMin = 0.8f;
    public float scaleMax = 1.2f;
    public float rotationRange = 30f;
    public float speedMin = 1.0f;
    public float speedMax = 3.0f;
    public float rotationSpeed = 20f;
}
