using UnityEngine;

namespace Player_Script
{
    public class FPActivity : Activity
    {
        protected FPController controller;
        protected FPControllerPreset preset;

        protected virtual void Awake()
        {
            controller = GetComponentInParent<FPController>();

            if (controller == null)
            {
                Debug.LogError($"{GetType().Name} could not find FPController in parent of {gameObject.name}");
            }

            preset = controller.preset;

            if (preset == null)
            {
                Debug.LogError($"{GetType().Name} could not find FPControllerPreset on {controller.name}");
            }
        }
    }
}

// This has references to the fp controller and fp contoller module