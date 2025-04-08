using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    public Camera playerCamera; // 手動設定 Camera
    public Transform holdPosition; // 物品拿取位置
    private Grabbable grabbedObject; // 目前拿起的物品

    void Update()
    {
        // 確保 Camera 存在
        if (playerCamera == null)
        {
            playerCamera = FindObjectOfType<Camera>();
            if (playerCamera == null)
            {
                Debug.LogWarning("⚠️ Camera 尚未生成，等待下一幀再嘗試...");
                return;
            }
            Debug.Log("✅ 已找到 Camera：" + playerCamera.gameObject.name);
        }

        // 按下滑鼠左鍵，嘗試拾取
        if (Input.GetMouseButtonDown(0))
        {
            if (grabbedObject == null)
            {
                TryPickup();
            }
            else
            {
                DropObject();
            }
        }

        // 如果正在抓著物品，就讓它跟著滑鼠移動
        if (grabbedObject != null)
        {
            MoveGrabbedObjectWithMouse();
        }
    }

    void TryPickup()
    {
        if (playerCamera == null) return;

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * 5f, Color.red, 1f); // 畫出射線

        if (Physics.Raycast(ray, out hit, 10f))
        {
            Debug.Log("🔹 射線擊中：" + hit.collider.gameObject.name);

            // 嘗試從點到的物體或其父物體取得 Grabbable 組件
            Grabbable grabbable = hit.collider.GetComponent<Grabbable>();
            if (grabbable == null)
            {
                grabbable = hit.collider.GetComponentInParent<Grabbable>();
            }

            if (grabbable != null)
            {
                Debug.Log("✅ 找到可拾取物品：" + grabbable.gameObject.name);
                grabbedObject = grabbable;
                grabbedObject.Grab();
            }
            else
            {
                Debug.LogWarning($"⚠️ 擊中物體「{hit.collider.gameObject.name}」，但找不到 `Grabbable` 組件（也檢查了父物件）");
            }
        }
        else
        {
            Debug.Log("❌ 沒有擊中任何物體");
        }
    }

    void DropObject()
    {
        if (grabbedObject != null)
        {
            grabbedObject.Release();
            grabbedObject = null;
        }
    }

    void MoveGrabbedObjectWithMouse()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPosition = ray.origin + ray.direction * 3f; // 你可以調整這個距離
        grabbedObject.MoveTo(targetPosition);
    }
}







