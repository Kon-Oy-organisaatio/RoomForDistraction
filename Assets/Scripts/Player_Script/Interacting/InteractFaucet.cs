using UnityEngine;

public class InteractFaucet : InteractObject
{
    [SerializeField] StopEmmit stopEmitScript;

    public override void Interact()
    {
        base.Interact();
        stopEmitScript.TogleEmmit();
    }
}
