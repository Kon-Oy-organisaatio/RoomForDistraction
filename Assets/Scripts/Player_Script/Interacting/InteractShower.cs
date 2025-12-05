using UnityEngine;

public class InteractShower : InteractObject
{
    [SerializeField] StopEmmit stopEmitScript;
    public override void Interact()
    {
        state = !state;
        stopEmitScript.TogleEmmit();
    }

}
