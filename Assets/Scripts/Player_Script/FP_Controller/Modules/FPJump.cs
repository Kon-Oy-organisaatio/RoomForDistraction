using UnityEngine;

namespace Player_Script
{
    public class FPJump : FPControllerModule
    {
        [SerializeField] float coyoteTimer = 0f;

        private void Start()
        {
            controller.TryJump += OnTryJump;
            controller.Landed.AddListener(OnLanded);
        }

        void OnLanded()
        {
            // Needed for landing logic but dont remeber why
        }

        private void Update()
        {
            if (controller == null)
            {
                Debug.LogError("FPJump: controller is NULL on " + gameObject.name);
                return;
            }


            if (controller.Grounded)
            {
                coyoteTimer = 0f;
            }
            else
            {
                coyoteTimer += Time.deltaTime;
            }
        }

        private void OnTryJump()
        {
            if (coyoteTimer <= preset.coyoteTime)
            {
                Jump();

                controller.Jumped?.Invoke();

                return;
            }
        }

        private void Jump()
        {
            controller.verticalVelocity = Mathf.Sqrt(preset.jumpHeight * -2f * Physics.gravity.y * preset.gravityScale);
        }
    }
}