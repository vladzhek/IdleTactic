using System;
using UnityEngine;
using Zenject;

namespace Slime.Services
{
    [RequireComponent(typeof(Camera))]
    public class CameraComponent : MonoBehaviour
    {
        [SerializeField] private ECameraType _cameraType;

        [Inject]
        public void Construct(ICameraService cameraService)
        {
            switch (_cameraType)
            {
                case ECameraType.Main:
                    cameraService.SetMainCamera(GetComponent<Camera>());
                    break;
                case ECameraType.UI:
                    cameraService.SetUICamera(GetComponent<Camera>());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    internal enum ECameraType
    {
        Main,
        UI,
    }
}