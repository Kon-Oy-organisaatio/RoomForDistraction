using UnityEngine;

namespace Player_Script
{
    public class FPCameraLand : FPActivity
    {
        enum EState
        {
            Land,
            Recovery
        }

        EState state = EState.Land;
        private float stateTimer = 0f;

        [SerializeField, Min(0f)] float maxOffset = 1.0f;
        [SerializeField, Min(1f)] float maxVerticalSpeed = 15f;

        [Header("Land State")]
        [SerializeField, Min(0.01f)] float landDuration = 0.2f;
        [SerializeField] AnimationCurve landCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [Header("Recovery State")]
        [SerializeField] float recoverySmoothness = 1f;

        private Vector3 targetOffset;
        private Vector3 currentOffset;

        protected override void Awake()
        {
            base.Awake();

            controller.Landed.AddListener(OnLanded);

            controller.RequestCameraOffset += () =>
            {
                controller.cameraPositionOffset += currentOffset;
            };
        }

        private void Update()
        {
            stateTimer += Time.deltaTime;

            if (Active)
            {
                if(state == EState.Land)
                {
                    float t = landCurve.Evaluate(stateTimer / landDuration);
                    currentOffset = Vector3.LerpUnclamped(Vector3.zero, targetOffset, t);
                }
                else if(state == EState.Recovery)
                {
                    currentOffset = Vector3.Lerp(currentOffset, Vector3.zero, Time.deltaTime * recoverySmoothness);
                }

                if (state == EState.Land && stateTimer >= landDuration)
                {
                    ChangeState(EState.Recovery);
                }
                else if (state == EState.Recovery && Vector3.Distance(currentOffset, Vector3.zero) <= 0.05f)
                {
                    TryStop(this);
                }
            }
        }

        protected override void StopActivity()
        {
            controller.cameraPositionOffset = Vector3.zero;
        }

        private void OnLanded()
        {
            float scale = Mathf.InverseLerp(0f, maxVerticalSpeed, Mathf.Abs(controller.verticalVelocity));

            targetOffset = new Vector3(0f, -maxOffset * scale, 0f);

            TryStart(this);
            ChangeState(EState.Land);
        }

        private void ChangeState(EState newState)
        {
            state = newState;

            stateTimer = 0f;
        }
    }
}