using UnityEngine;

namespace Player_Script
{
    public class FPCrouch : FPActivity
    {
        protected override void Awake()
        {
            base.Awake();

            controller.Crouch = this;
        }

        public override bool CanStartActivity()
        {
            if (IsActive(controller.Sprint))
            {
                return false;
            }

            return true;
        }

        public override bool CanStopActivity()
        {
            Ray ray = new Ray(controller.CameraTransform.position, Vector3.up);

            float standingHeight = 2f;
            float raycastDistance = standingHeight - controller.CameraTransform.localPosition.y;

            raycastDistance = Mathf.Max(0f, raycastDistance);

            if(Physics.Raycast(ray, raycastDistance, preset.obstacleLayerMask))
            {
                return false;
            }
            return true;
        }
    }
}
