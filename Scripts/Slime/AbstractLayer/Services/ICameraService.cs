using UnityEngine;

namespace Slime.Services
{
    public interface ICameraService
    {
        Camera MainCamera { get; }
        Camera UiCamera { get; }
        void SetMainCamera(Camera mainCamera);
        void SetUICamera(Camera uiCamera);
        void Shake();
        void SetMainCameraTarget(Transform transform);
    }
}