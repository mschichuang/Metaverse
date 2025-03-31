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
    }

    void TryPickup()
    {
        if (playerCamera == null) return;

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * 5f, Color.red, 1f); // 🔥 在場景視圖畫出 Ray

        if (Physics.Raycast(ray, out hit, 10f)) // 🔺 將射線範圍改為 5 公尺
        {
            Debug.Log("🔹 Raycast 擊中了：" + hit.collider.gameObject.name);

            Grabbable grabbable = hit.collider.GetComponent<Grabbable>();
            if (grabbable != null)
            {
                Debug.Log("✅ 找到可拾取物品：" + grabbable.gameObject.name);
                grabbedObject = grabbable;
                grabbedObject.Grab(holdPosition);
            }
            else
            {
                Debug.Log("⚠️ 擊中物體，但沒有 `Grabbable` 組件：" + hit.collider.gameObject.name);
            }
        }
        else
        {
            Debug.Log("❌ Raycast 沒有擊中任何物體");
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
}

