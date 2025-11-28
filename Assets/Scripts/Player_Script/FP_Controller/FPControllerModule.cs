using UnityEngine;

namespace Player_Script
{
    public class FPControllerModule : MonoBehaviour
    {
        [SerializeField]
        protected FPController controller;

        [SerializeField]
        protected FPControllerPreset preset => controller.preset;

        protected virtual void Awake()
        {
            controller = GetComponentInParent<FPController>();

            if (controller == null)
            {
                Debug.LogError($"{GetType().Name} could not find FPController in parent of {gameObject.name}");
            }
        }
    }
}