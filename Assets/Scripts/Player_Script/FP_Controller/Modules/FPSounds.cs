using UnityEngine;

namespace Player_Script
{
    public class FPSounds : FPControllerModule
    {
        [SerializeField] AudioSource audioSource;

        [SerializeField] AudioClip[] jumpSounds;
        [SerializeField] AudioClip[] landSounds;
        [SerializeField] AudioClip[] footstepSounds;

        [SerializeField] float footstepTimer = 0f;


        private void Start()
        {
            controller.Jumped.AddListener(PlayJumpSound);
            controller.Landed.AddListener(OnLanded);
        }

        private void Update()
        {
            UpdateFootstepSounds();
        }

        private void UpdateFootstepSounds()
        {
            if (controller.Grounded && controller.currentSpeed >= 0.2f)
            {
                float rate = preset.footstepWalkRate;
                float volume = preset.footstepWalkVolume;

                if (controller.crouching)
                {
                    rate = preset.footstepCrouchRate;
                    volume = preset.footstepCrouchVolume;
                }
                else
                {
                    float t = Mathf.InverseLerp(preset.walkSpeed, preset.sprintSpeed, controller.currentSpeed);

                    rate = Mathf.Lerp(preset.footstepWalkRate, preset.footstepSprintRate, t);
                    volume = Mathf.Lerp(preset.footstepWalkVolume, preset.footstepSprintVolume, t);
                }

                if (Time.time >= footstepTimer)
                {
                    PlaySound(footstepSounds, volume);
                    footstepTimer = Time.time + rate;
                }
            }
        }

        void OnLanded()
        {
            float scale = Mathf.InverseLerp(0f, -20f, controller.verticalVelocity);

            scale *= preset.maxLandSoundVolume;

            PlaySound(landSounds, scale);
        }

        private void PlaySound(AudioClip[] clips, float volumeScale = 1f)
        {
            try
            {
                int index = Random.Range(0, clips.Length);
                AudioClip clip = clips[index];

                audioSource.PlayOneShot(clip, volumeScale);
            }
            catch { }
        }

        private void PlayJumpSound()
        {


            PlaySound(jumpSounds, preset.jumpSoundVolume);
        }
    }
}