using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace AzizStuff
{
    [RequireComponent(typeof(Camera))]
    public class CameraZoom : MonoBehaviour
    {
        [Tooltip("Orthographic size at full zoom-in. Smaller = closer to the hero.")]
        [Min(0.1f)] public float minSize = 3f;

        [Tooltip("Orthographic size at full zoom-out. Larger = wider field of view.")]
        [Min(0.1f)] public float maxSize = 8f;

        [Tooltip("Orthographic size change per scroll notch.")]
        [Min(0f)] public float zoomStep = 0.5f;

        [Tooltip("Higher = snappier; 0 = instant snap.")]
        [Min(0f)] public float zoomSmoothing = 12f;

        Camera _cam;
        float _targetSize;

        void Awake()
        {
            _cam = GetComponent<Camera>();
            _targetSize = _cam.orthographicSize;
        }

        void Update()
        {
            if (Mouse.current == null) return;

            // Skip zoom when the cursor is over a UGUI element so scrolling UI lists doesn't move the camera.
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

            float scrollY = Mouse.current.scroll.ReadValue().y;
            if (Mathf.Abs(scrollY) > 0.01f)
            {
                _targetSize -= Mathf.Sign(scrollY) * zoomStep;
                _targetSize = Mathf.Clamp(_targetSize, minSize, maxSize);
            }

            _cam.orthographicSize = zoomSmoothing > 0f
                ? Mathf.Lerp(_cam.orthographicSize, _targetSize, Time.unscaledDeltaTime * zoomSmoothing)
                : _targetSize;
        }
    }
}
