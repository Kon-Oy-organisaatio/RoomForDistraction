using UnityEngine;

namespace Player_Script
{
    [System.Serializable]
    public class HeadBobPreset
    {
        public float amplitude = 0.5f;
        public float frequency = 5f;

        [Space(10)]
        public float noiseScale = 1f;
        public float maxRotation = 30f;

    }
    public class FPHeadbob : FPControllerModule
    {
        private float amplitude = 0.5f;
        private float frequency = 5f;
        private float noiseScale = 1f;
        private float maxRotation = 30f;

        private Vector3 positionOffset;
        private Quaternion rotationOffset = Quaternion.identity;

        private float angle = 0f;
        private float speedRatio = 0f;

        [SerializeField] bool applyPositionOffset = true;
        [SerializeField] bool applyRotationOffset = true;
        [SerializeField] bool applyRotationNoiseOffset = true;

        public void SetValues(HeadBobPreset preset)
        {
            amplitude = preset.amplitude;
            frequency = preset.frequency;
            noiseScale = preset.noiseScale;
            maxRotation = preset.maxRotation;
        }

        protected override void Awake()
        {
            base.Awake();

            controller.RequestCameraOffset += () =>
            {
                controller.cameraPositionOffset += positionOffset;
                controller.cameraRotationOffset *= rotationOffset;
            };
        }

        private void Update()
        {
            if (controller.Grounded)
            {
                UpdateValues();
                UpdateHeadBobbing();  
            }
            else
            {
                positionOffset = Vector3.Lerp(positionOffset, Vector3.zero, 10f * Time.deltaTime);
                rotationOffset = Quaternion.Lerp(rotationOffset, Quaternion.identity, 10f * Time.deltaTime);
            }
        }

        private void UpdateValues()
        {
            if (Activity.IsActive(controller.Sprint))
            {
                SetValues(preset.headBobSprint);
                speedRatio = Mathf.InverseLerp(preset.walkSpeed, preset.sprintSpeed, controller.currentSpeed);
            }
            else if (Activity.IsActive(controller.Crouch))
            {
                SetValues(preset.headBobCrouch);
                speedRatio = Mathf.InverseLerp(0f, preset.crouchSpeed, controller.currentSpeed);
            }
            else
            {
                SetValues(preset.headBobWalk);
                speedRatio = Mathf.InverseLerp(0f, preset.walkSpeed, controller.currentSpeed);
            }
        }

        private void UpdateHeadBobbing()
        {
            angle += Time.deltaTime * frequency * speedRatio;

            float bobX = Mathf.Sin(angle) * amplitude * speedRatio;

            // float bobY = Mathf.Cos(angle) * Mathf.Sin(angle) * amplitude * speedRatio;
            float bobY = Mathf.Cos(angle * 2f) * amplitude * speedRatio;

            if (applyPositionOffset)
            {
                Vector3 targetOffset = new Vector3(bobX, bobY, 0f);
                positionOffset = Vector3.Lerp(positionOffset, targetOffset, 10 * Time.deltaTime);
            }

            float bobAngle = maxRotation * Mathf.Sin(angle);
            float noise = Mathf.PerlinNoise1D(angle * noiseScale);

            if(applyRotationOffset == false)
            {
                bobAngle = 0f;
            }

            if(applyRotationNoiseOffset == false)
            {
                bobAngle *= noise;
            }

            Quaternion stabilizationRotation = Quaternion.LookRotation(Vector3.forward * 30f - positionOffset);

            Quaternion targetRotation = stabilizationRotation * Quaternion.Euler(0f, 0f, bobAngle * speedRatio);

            rotationOffset = Quaternion.Lerp(rotationOffset, targetRotation, 10f * Time.deltaTime);
        }
    }
}

