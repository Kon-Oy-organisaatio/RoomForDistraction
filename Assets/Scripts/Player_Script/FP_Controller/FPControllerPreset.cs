using UnityEngine;

namespace Player_Script
{
    [CreateAssetMenu(menuName = "PlayerFP/FPControllerPreset")]
    public class FPControllerPreset : ScriptableObject
    {
        [Header("Movement Parameters")]
        public float acceleration = 15f;

        [Space(15)]
        public float crouchSpeed = 2f;
        public float walkSpeed = 3.5f;
        public float sprintSpeed = 10f;

        [Header("Jump Parameters")]
        [Tooltip("This is how high the character can jump")]
        public float jumpHeight = 2f;
        public float coyoteTime = 0.1f;

        [Header("Looking Parameters")]
        public Vector2 lookSensitivity = new Vector2(0.1f, 0.1f);
        public float pitchLimit = 85f;

        [Header("Camera Parameters")]
        public float cameraNormalFOV = 60f;
        public float cameraSprintFOV = 67.5f;
        public float cameraFOVSmoothing = 5f;

        [Header("Camera heigts and radius")]
        public float playerCameraStandingHeight = 1.6f;
        public float playerCameraCrouchingHeight = 0.9f;
        public float characterControllerRadius = 0.2f;

        [Header("Head Bobbing")]
        public HeadBobPreset headBobWalk;
        public HeadBobPreset headBobCrouch;
        public HeadBobPreset headBobSprint;

        [Header("Physics Parameters")]
        public float gravityScale = 2f;
        public LayerMask obstacleLayerMask = Physics.DefaultRaycastLayers;

        [Header("Sounds & Footstep Parameters")]
        public float footstepCrouchRate = 1f;
        public float footstepWalkRate = 0.8f;
        public float footstepSprintRate = 0.3f;

        [Space(15)]
        public float footstepCrouchVolume = 0.07f;
        public float footstepWalkVolume = 0.1f;
        public float footstepSprintVolume = 0.5f;

        [Space(15)]
        public float maxLandSoundVolume = 0.7f;
        public float jumpSoundVolume = 0.5f;

        [Header("Interaction")]
        public float interactionDistance = 5f;
    }
}