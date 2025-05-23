using UnityEngine;
using SpatialSys.UnitySDK;

public class DragAndDrop : MonoBehaviour
{
    private bool isDragging = false;
    private float distanceFromCamera;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = SpatialBridge.cameraService.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    isDragging = true;
                    distanceFromCamera = hit.distance;
                    
                }
            }
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            Ray ray = SpatialBridge.cameraService.ScreenPointToRay(Input.mousePosition);
            Vector3 targetPos = ray.GetPoint(distanceFromCamera);
            transform.position = targetPos;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
}