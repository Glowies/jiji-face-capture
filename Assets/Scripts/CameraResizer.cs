using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraResizer : MonoBehaviour
{
    public MeshRenderer TargetRenderer;
    public float CameraDistance = 2;

    private Camera _camera;

    void Start()
    {
        TryGetComponent(out _camera);
    }

    // Adjust the camera's height so the desired scene width fits in view
    // even if the screen/window size changes dynamically.
    void Update()
    {
        // source: https://forum.unity.com/threads/fit-object-exactly-into-perspective-cameras-field-of-view-focus-the-object.496472/

        var bounds = TargetRenderer.bounds;
        Vector3 objectSizes = bounds.max - bounds.min;
        float objectSize = Mathf.Max(objectSizes.x, objectSizes.y, objectSizes.z);

        float aspectRatio = Screen.width / (float)Screen.height;
        if (aspectRatio < 1)
        {
            objectSize /= aspectRatio;
        }

        float cameraView = 2.0f * Mathf.Tan(0.5f * Mathf.Deg2Rad * _camera.fieldOfView); // Visible height 1 meter in front
        float distance = CameraDistance * objectSize / cameraView; // Combined wanted distance from the object
        distance += 0.5f * objectSize; // Estimated offset from the center to the outside of the object
        _camera.transform.position = bounds.center - distance * _camera.transform.forward;
    }
}
