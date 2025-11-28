using UnityEngine;

namespace Player_Script
{
    public class FPSprint : FPActivity
    {
        protected override void Awake()
        {
            base.Awake();

            controller.Sprint = this;
        }

        public override bool CanStartActivity()
        {
            bool crouch = Activity.IsActive(controller.Crouch);
            return !crouch;
        }

        private void Update()
        {
            if (controller.sprintInput && controller.currentSpeed > 0.1f)
            {
                TryStartActivity();
            }

           if (controller.sprintInput == false || controller.currentSpeed <= 0.1f)
            {
                TryStopActivity();
            }
        }
    }
}

