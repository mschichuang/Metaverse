using UnityEngine;
using SpatialSys.UnitySDK;

public class DragAndDrop : MonoBehaviour
{
    private bool isDragging = false;
    private float distanceFromCamera;
    private Vector3 offset;

    void Update()
    {
        // 滑鼠左鍵按下：選取物件
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = SpatialBridge.cameraService.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    isDragging = true;
                    distanceFromCamera = hit.distance;
                    offset = transform.position - hit.point;
                }
            }
        }

        // 拖曳中：移動物件
        if (isDragging && Input.GetMouseButton(0))
        {
            Ray ray = SpatialBridge.cameraService.ScreenPointToRay(Input.mousePosition);
            Vector3 targetPos = ray.GetPoint(distanceFromCamera) + offset;
            transform.position = targetPos;
        }

        // 滑鼠放開：嘗試吸附
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            TrySnapToSocket();
        }
    }

    // 嘗試吸附到最近的 Socket
    private void TrySnapToSocket()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 0.2f); // 半徑可微調
        foreach (Collider col in hits)
        {
            if (col.CompareTag("Socket"))
            {
                transform.position = col.transform.position;
                transform.rotation = col.transform.rotation;
                return; // 找到一個就停止
            }
        }
    }
}







