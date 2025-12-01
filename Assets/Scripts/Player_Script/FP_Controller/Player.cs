using UnityEngine.InputSystem;
using UnityEngine;

namespace Player_Script
{
    [RequireComponent(typeof(FPController))]
    public class Player : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] FPController fpController;

        #region Input Handling
        // NOTE: On(name) where name needs to match the inpithandlers name

        // Movement
        private void OnMove(InputValue value)
        {
            fpController.moveInput = value.Get<Vector2>();
        }

        // Looking around
        private void OnLook(InputValue value)
        {
            fpController.lookInput = value.Get<Vector2>();
        }

        // Sprinting (possible boost) 
        /*
        private void OnSprint(InputValue value)
        {
            fpController.sprintInput = value.isPressed;
        }
        */

        // Crouching
        private void OnCrouch(InputValue value)
        {
            if (value.isPressed)
            {
                Activity.TryToggle(fpController.Crouch);
            }
        }

        // Interacting
        private void OnInteract(InputValue value)
        {
            if (value.isPressed)
            {
                fpController.TryInteract?.Invoke();
            }
        }

        /*
        private void OnJump(InputValue value)
        {
            if (value.isPressed)
            {
                fpController.TryJump?.Invoke();
            }
        }
        */

        #endregion

        #region Unity Methods
        private void OnValidate()
        {
            if (fpController == null) fpController = GetComponent<FPController>();
        }

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            if (Time.timeScale == 0f)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        #endregion
    }
}

