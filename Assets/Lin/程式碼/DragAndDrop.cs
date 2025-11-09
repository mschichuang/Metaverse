using UnityEngine;
using SpatialSys.UnitySDK;

public class DragAndDrop : MonoBehaviour
{
    private bool isDragging = false;
    private float distanceFromCamera;

    // 可以在 Inspector 指派 Unity Editor 專用相機（非 Spatial）
    [SerializeField] private Camera editorCamera;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = GetActiveRay();
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
            Ray ray = GetActiveRay();
            Vector3 targetPos = ray.GetPoint(distanceFromCamera);
            transform.position = targetPos;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    private Ray GetActiveRay()
    {
        // Spatial 環境用 Spatial 相機
        if (SpatialBridge.cameraService != null)
            return SpatialBridge.cameraService.ScreenPointToRay(Input.mousePosition);

        // Unity 編輯器環境 → 使用 Inspector 指派的相機
        if (editorCamera != null)
            return editorCamera.ScreenPointToRay(Input.mousePosition);

        // 沒有相機 → 拋出警告
        Debug.LogWarning("請在 Inspector 指派 Editor Camera！");
        return new Ray(Vector3.zero, Vector3.forward);
    }
}




