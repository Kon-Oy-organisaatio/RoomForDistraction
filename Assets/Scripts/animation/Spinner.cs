using UnityEngine;

public class Spinner : MonoBehaviour
{
    public float rotationSpeed = 6f;
    public Vector3 rotationAxis = Vector3.up;
    public bool invertDirection = true;

    void Update()
    {
        float direction = invertDirection ? -1f : 1f;
        transform.Rotate(rotationAxis, rotationSpeed * direction * Time.deltaTime);
    }
}
