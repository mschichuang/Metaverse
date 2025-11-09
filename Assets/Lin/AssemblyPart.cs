using UnityEngine;
using SpatialSys.UnitySDK;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class AssemblyPart : MonoBehaviour
{
    [Tooltip("零件的ID，用來跟Socket匹配")]
    public string partID;

    private Rigidbody rb;
    private bool isGrabbed = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Spatial 抓取開始
    public void OnGrab()
    {
        isGrabbed = true;
        rb.isKinematic = true;  // 抓起時暫停物理
    }

    // Spatial 放開
    public void OnRelease()
    {
        isGrabbed = false;
        rb.isKinematic = false; // 放開時恢復物理

        TrySnapToSocket();
    }

    // 嘗試吸附到插槽
    private void TrySnapToSocket()
    {
        float snapRadius = 0.25f;
        Collider[] hits = Physics.OverlapSphere(transform.position, snapRadius);

        foreach (var hit in hits)
        {
            AssemblySocket1 socket = hit.GetComponent<AssemblySocket1>();
            if (socket != null && socket.allowedPartID == partID)
            {
                // ✅ 對應成功 → 吸附位置
                transform.position = socket.transform.position;

                // ✅ 保留原角度（不改旋轉）
                // 如果要轉向，可改成：
                // transform.rotation = socket.transform.rotation;

                // ✅ 設為子物件（固定）
                transform.SetParent(socket.transform);

                // ✅ 鎖定不再掉落
                rb.isKinematic = true;
                rb.useGravity = false;

                Debug.Log($"✅ {name} 成功插入 {socket.name}");
                return;
            }
        }

        Debug.Log($"❌ {name} 沒有找到匹配插槽");
    }
}

