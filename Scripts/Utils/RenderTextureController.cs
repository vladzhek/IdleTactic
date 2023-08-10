using UnityEngine;

// NOTE: different render textures? caching?

namespace Utils
{
    public class RenderTextureController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _cameraTarget;
        
        private const string LAYER_NAME = "RenderTexture";
        
        private int Layer { get; set; } = -1;

        private void Awake()
        {
            Layer = LayerMask.NameToLayer(LAYER_NAME);
        }

        // ReSharper disable once ParameterHidesMember
        public void RenderAsCharacter(Transform transform)
        {
            MoveToLayer(transform);
            MoveToCamera(transform);
            FocusCamera(transform);
        }

        // ReSharper disable once ParameterHidesMember
        private void MoveToCamera(Transform transform)
        {
            foreach (Transform child in _cameraTarget)
            {
                Destroy(child.gameObject);
            }

            transform.parent = _cameraTarget;
            transform.position = Vector3.zero;
            transform.rotation = default;
        }
        
        private void MoveToLayer(Component component)
        {
            if (Layer == -1)
            {
                Logger.Warning("layer not initialized");
            }
            
            var children = component.GetComponentsInChildren<Transform>(true);
            foreach (var child in children)
            {
                child.gameObject.layer = Layer;
            }
        }

        private static Bounds GetBounds(Component component)
        {
            // GetComponentsInChildren() also returns components on gameobject which you call it on
            // you don't need to get component specially on gameObject
            var renderers = component.GetComponentsInChildren<Renderer>();
 
            // If renderers.Length = 0, you'll get OutOfRangeException
            // or null when using Linq's FirstOrDefault() and try to get bounds of null
            var bounds = renderers.Length > 0 ? renderers[0].bounds : new Bounds();
 
            // Or if you like using Linq
            // Bounds bounds = renderers.Length > 0 ? renderers.FirstOrDefault().bounds : new Bounds();
 
            // Start from 1 because we've already encapsulated renderers[0]
            for (var i = 1; i < renderers.Length; i++)
            {
                if (renderers[i].enabled)
                {
                    bounds.Encapsulate(renderers[i].bounds);
                }
            }
 
            return bounds;
        }

        // ReSharper disable once ParameterHidesMember
        private static void FocusCamera(Transform transform)
        {
            // TODO: calculate dynamically
            transform.position = new Vector3(0.25f, -3.25f, 10f);

            /*var targetTexture = _camera.targetTexture;
            var width = targetTexture.width / 2;
            var height = targetTexture.height / 2;
            var point = new Vector3(width, height, _camera.nearClipPlane);
            transform.position = _camera.ScreenToWorldPoint(point);*/

            //var bounds = GetBounds(transform);

            /*var maxExtent = bounds.extents.magnitude;
            var minDistance = maxExtent / Mathf.Sin(Mathf.Deg2Rad * _camera.fieldOfView / 2f);
            _camera.transform.position = transform.position - Vector3.forward * minDistance;
            _camera.nearClipPlane = minDistance - maxExtent;*/

            /*var virtualSphereRadius = Vector3.Magnitude(bounds.max - bounds.center);
            var minD = virtualSphereRadius / Mathf.Sin(Mathf.Deg2Rad * _camera.fieldOfView / 2);
            var cameraTransform = _camera.transform;
            var cameraPosition = cameraTransform.position;
            var vector = (cameraPosition - bounds.center) / Vector3.Magnitude(cameraPosition -  bounds.center);
            cameraTransform.position =  minD * vector;
            cameraTransform.LookAt(bounds.center);
            _camera.nearClipPlane = minD - virtualSphereRadius;*/

            /*var centerAtFront = new Vector3(bounds.center.x, bounds.center.y, bounds.max.z);
            var centerAtFrontTop = new Vector3(bounds.center.x, bounds.max.y, bounds.max.z);
            var centerToTopDist = (centerAtFrontTop - centerAtFront).magnitude;
            var minDistance = centerToTopDist / Mathf.Tan(_camera.fieldOfView * .5f * Mathf.Deg2Rad);
            var cameraTransform = _camera.transform;
            cameraTransform.position = new Vector3(bounds.center.x, bounds.center.y, -minDistance);
            cameraTransform.LookAt(bounds.center);*/
        }
    }
}