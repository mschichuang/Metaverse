using UnityEngine;
using SpatialSys.UnitySDK;

[RequireComponent(typeof(UnlockManager))]
public class DragAndDrop : MonoBehaviour
{
    private bool isDragging = false;
    private float distanceFromCamera;
    private UnlockManager unlockManager;

    void Start()
    {
        // 取得同一物件上的 UnlockManager
        unlockManager = GetComponent<UnlockManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = SpatialBridge.cameraService.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    // 只有解鎖物品才能開始拖曳
                    if (unlockManager.IsUnlocked())
                    {
                        isDragging = true;
                        distanceFromCamera = hit.distance;
                    }
                    else
                    {
                        Debug.Log($"{gameObject.name} 還沒解鎖，不能移動！");
                    }
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
