using UnityEngine;
using System.Collections.Generic;
using SpatialSys.UnitySDK;

public class DragAndDropManager1 : MonoBehaviour
{
    private static List<GameObject> draggableObjects = new List<GameObject>();
    private GameObject selectedObject;
    private float distanceFromCamera;

    public static void Register(GameObject obj)
    {
        if (!draggableObjects.Contains(obj))
            draggableObjects.Add(obj);
    }

    void Update()
    {
        // 🖱️ 滑鼠按下：檢查是否點到可拖曳物件
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = SpatialBridge.cameraService.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (draggableObjects.Contains(hit.transform.gameObject))
                {
                    selectedObject = hit.transform.gameObject;
                    distanceFromCamera = hit.distance;
                }
            }
        }

        // ✋ 拖曳中：移動物件
        if (selectedObject != null && Input.GetMouseButton(0))
        {
            Ray ray = SpatialBridge.cameraService.ScreenPointToRay(Input.mousePosition);
            Vector3 targetPos = ray.GetPoint(distanceFromCamera);
            selectedObject.transform.position = targetPos;
        }

        // 🧩 放開滑鼠：嘗試組裝
        if (Input.GetMouseButtonUp(0) && selectedObject != null)
        {
            TryAssemble(selectedObject);
            selectedObject = null;
        }
    }

    // 🧠 嘗試組裝邏輯
    private void TryAssemble(GameObject partObj)
    {
        DraggablePart part = partObj.GetComponent<DraggablePart>();
        if (part == null || part.anchorPoint == null)
            return;

        float checkRadius = 0.3f; // 組裝判定距離
        Collider[] nearby = Physics.OverlapSphere(part.anchorPoint.position, checkRadius);

        foreach (var col in nearby)
        {
            AssemblySocket socket = col.GetComponent<AssemblySocket>();
            if (socket == null)
                continue;

            // ✅ 零件 ID 符合才能組裝
            if (socket.allowedPartID == part.partID)
            {
                // ✅ 只吸附位置，不改變原本旋轉
                partObj.transform.position = socket.transform.position;

                // ✅ 設為子物件（若啟用）
                if (socket.attachAsChild)
                    partObj.transform.SetParent(socket.transform);

                // ✅ 鎖定剛體
                if (socket.lockTransform)
                {
                    Rigidbody rb = partObj.GetComponent<Rigidbody>();
                    if (rb != null)
                        rb.isKinematic = true;
                }

                Debug.Log($"✅ {partObj.name} 成功插入 {socket.name}");
                return;
            }
        }

        Debug.Log($"❌ {partObj.name} 沒有插入任何匹配的插槽");
    }
}
