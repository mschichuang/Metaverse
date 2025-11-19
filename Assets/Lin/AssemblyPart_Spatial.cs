using UnityEngine;
using SpatialSys.UnitySDK;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class AssemblyPart_Spatial : MonoBehaviour
{
    [Tooltip("零件ID，必須和插槽的 allowedPartID 一致")]
    public string partID;

private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false; // 初始可被抓取
        rb.useGravity = true;
    }

    // Spatial Grab Start 事件綁定
    public void OnGrab()
    {
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    // Spatial Grab End 事件綁定
    public void OnRelease()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        TrySnapToSocket();
    }

    private void TrySnapToSocket()
    {
        float snapRadius = 0.6f;
        Collider[] hits = Physics.OverlapSphere(transform.position, snapRadius);

        foreach (var hit in hits)
        {
            AssemblySocket_Spatial socket = hit.GetComponent<AssemblySocket_Spatial>();
            if (socket != null && socket.allowedPartID == partID)
            {
                Transform snap = socket.snapPoint != null ? socket.snapPoint : socket.transform;

                transform.position = snap.position;
                transform.rotation = snap.rotation;
                transform.SetParent(socket.transform);

                rb.isKinematic = true;
                rb.useGravity = false;

                Debug.Log($"{name} 成功插入 {socket.name}");
                return;
            }
        }

        Debug.Log($"{name} 沒有找到匹配插槽");
    }

    // 可選：畫出吸附範圍
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.6f);
    }


}
