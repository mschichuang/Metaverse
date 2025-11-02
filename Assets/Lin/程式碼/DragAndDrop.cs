using UnityEngine;
using SpatialSys.UnitySDK; // 若沒有 Spatial，可以用 Camera.main 的替代分支

public class DragAndDrop : MonoBehaviour
{
    private bool isDragging = false;
    private float distanceFromCamera;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    [Header("Part Settings")]
    public string partID = "part";     // 在 Inspector 指定 e.g. "wheel"
    public float snapCheckRadius = 0.5f;

    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        // 1. Mouse down -> 檢查是否點到自己
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = GetScreenRay();
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    isDragging = true;
                    distanceFromCamera = hit.distance;
                }
            }
        }

        // 2. 拖曳中
        if (isDragging && Input.GetMouseButton(0))
        {
            Ray ray = GetScreenRay();
            Vector3 target = ray.GetPoint(distanceFromCamera);
            transform.position = target;
        }

        // 3. 放開滑鼠 -> 嘗試組裝
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            if (!TrySnapToSocket())
            {
                // 組裝失敗 -> 回原位（也可以改成彈回效果）
                transform.position = originalPosition;
                transform.rotation = originalRotation;
            }
        }
    }

    // 嘗試找附近 socket 並組裝
    bool TrySnapToSocket()
    {
        Collider[] overlaps = Physics.OverlapSphere(transform.position, snapCheckRadius);
        foreach (var col in overlaps)
        {
            AssemblySocket socket = col.GetComponent<AssemblySocket>();
            if (socket != null && socket.allowedPartID == partID)
            {
                // 成功 -> 對齊 socket
                transform.position = socket.transform.position;
                transform.rotation = socket.transform.rotation;

                if (socket.attachAsChild)
                    transform.SetParent(socket.transform);

                if (socket.lockTransform)
                {
                    // 如果有 Rigidbody，讓它變成 kinematic 且不受影響
                    Rigidbody rb = GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                    }
                }

                return true;
            }
        }
        return false;
    }

    // 取得射線（支援 SpatialBridge 或 fallback）
    Ray GetScreenRay()
    {
        // 如果有 SpatialBridge 可用則用 Spatial 的相機服務
#if UNITY_EDITOR || !SPATIALBRIDGE_AVAILABLE
        if (Camera.main != null)
            return Camera.main.ScreenPointToRay(Input.mousePosition);
#endif

        // 預設嘗試 SpatialBridge（如果你在 Spatial 環境）
        return SpatialBridge.cameraService.ScreenPointToRay(Input.mousePosition);
    }

    // 可視化檢查範圍
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, snapCheckRadius);
    }
}



