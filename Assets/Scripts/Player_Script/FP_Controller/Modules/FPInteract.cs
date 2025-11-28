using TMPro;
using UnityEngine;

namespace Player_Script
{
    public class FPInteract : FPControllerModule
    {
        public Camera mainCamera;

        // This could be added to the FPController preset
        public float interactionDistance = 2f;

        public GameObject interactionUI;
        public TextMeshProUGUI interactionText;

        private IInteractable currentInteractable;

        void Start()
        {
            controller.TryInteract += OnTryInteract;

            // This might have to be changed if we have more than 1 camera idky
            mainCamera = Camera.main;
        }

        private void Update()
        {
            InteractionRay();
        }

        void OnTryInteract()
        {
            if (currentInteractable != null)
            {
                currentInteractable.Interact();
            }
        }

        void InteractionRay()
        {
            Ray ray = mainCamera.ViewportPointToRay(Vector3.one / 2f);
            RaycastHit hit;
            if ( currentInteractable != null) 
            {
                currentInteractable.HideOutline();
                currentInteractable = null;
            }

            bool hitSomething = false;


            if(Physics.Raycast(ray, out hit, interactionDistance))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                if(interactable != null && !interactable.IsDisabled())
                {
                    currentInteractable = interactable;
                    hitSomething = true;
                    interactionText.text = interactable.GetDescription();
                    currentInteractable.ShowOutline();
                }
            }
            interactionUI.SetActive(hitSomething);
        }

        public void OnDrawGizmos() {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(mainCamera.transform.position, mainCamera.transform.forward * interactionDistance);
        }
    }
}

