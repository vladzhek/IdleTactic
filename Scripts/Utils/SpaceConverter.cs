using UnityEngine;

namespace Utils
{
    public static class SpaceConverter
    {
        public static Vector3 ToScreenSpace(this Vector3 position, Camera projectionCamera)
        {
            var screenPoint = projectionCamera.WorldToScreenPoint(position);
            return screenPoint;
        }

        public static Vector3 ToCameraSpace(this Vector3 position, Camera projectionCamera)
        {
            var worldPoint = projectionCamera.ScreenToWorldPoint(position);
            return worldPoint;
        }

        public static Vector3 BetweenCameraSpace(this Vector3 position, Camera fromCamera, Camera toCamera)
        {
            var cameraPoint = ToScreenSpace(position, fromCamera);
            return ToCameraSpace(cameraPoint, toCamera);
        }
    }
}