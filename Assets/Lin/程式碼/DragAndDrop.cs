using UnityEngine;
using SpatialSys.UnitySDK;

[RequireComponent(typeof(UnlockManager))]
public class DragAndDrop : MonoBehaviour
{
    private bool isDragging = false;
    private float distanceFromCamera;
    private UnlockManager unlockManager;

    // 在 Inspector 指派 PurchaseUIManager（Canvas 上的腳本）
    public PurchaseUIManager purchaseUI;


    void Start()
    {
        unlockManager = GetComponent<UnlockManager>();
        if (purchaseUI == null)
            Debug.LogWarning("DragAndDrop: purchaseUI 未設定，未解鎖時無法顯示購買介面。");
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
                    if (unlockManager.IsUnlocked())
                    {
                        isDragging = true;
                        distanceFromCamera = hit.distance;
                    }
                    else
                    {
                        // 物品未解鎖 → 顯示購買提示 UI
                        if (purchaseUI != null)
                            purchaseUI.ShowPurchaseUI(unlockManager, unlockManager.price);
                        else
                            Debug.Log($"{gameObject.name} 未解鎖，且未設定 purchaseUI。");
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

