using UnityEngine;

public class StopEmmit : MonoBehaviour
{
    public ParticleSystem particleEffect;
    public bool stop = false;

    private void Update()
    {
        var emission = particleEffect.emission;

        if (stop)
        {
            emission.enabled = false;
        }
        else
        {
            emission.enabled = true;
        }
    }

    public void TogleEmmit()
    {
        if (stop)
        {
            stop = false;
        }
        else
        {
            stop = true;
        }
    }
}
