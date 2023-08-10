using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Slime.Services
{
    public class CameraService : ICameraService
    {
        public Camera MainCamera { get; private set; }
        public Camera UiCamera { get; private set; }

        private Tween _shakeTween;

        public void SetMainCamera(Camera mainCamera)
        {
            MainCamera = mainCamera;

            if (UiCamera != null)
            {
                MainCamera?.GetUniversalAdditionalCameraData().cameraStack.Add(UiCamera);
            }
        }

        public void SetUICamera(Camera uiCamera)
        {
            UiCamera = uiCamera;

            if (MainCamera != null)
            {
                MainCamera.GetUniversalAdditionalCameraData().cameraStack.Add(UiCamera);
            }
        }

        public void Shake()
        {
            MainCamera.GetComponentInChildren<CinemachineImpulseSource>().GenerateImpulse();
        }

        public void SetMainCameraTarget(Transform transform)
        {
            var virtualCamera = MainCamera.GetComponentInChildren<CinemachineVirtualCamera>();
            virtualCamera.m_Follow = transform;
            virtualCamera.m_LookAt = transform;
        }
    }
}