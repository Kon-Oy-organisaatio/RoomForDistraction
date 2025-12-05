using UnityEngine;
using UnityEngine.Android;

public class StopEmmit : MonoBehaviour
{
    public ParticleSystem waterFlow;
    public bool stop = false;

    private void Update()
    {
        var emission = waterFlow.emission;

        if (stop)
        {
            emission.enabled = false;
        }
        else
        {
            emission.enabled = true;
        }
    }

}
