using UnityEngine;
using SpatialSys.UnitySDK;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class AssemblyPart : MonoBehaviour
{
    [Tooltip("零件的ID，用來跟Socket匹配")]
    public string partID;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnGrab()
    {
        rb.isKinematic = true;
    }

    public void OnRelease()
    {
        rb.isKinematic = false;
        TrySnapToSocket();
    }

    private void TrySnapToSocket()
    {
        float snapRadius = 0.6f;
        Collider[] hits = Physics.OverlapSphere(transform.position, snapRadius);

        foreach (var hit in hits)
        {
            AssemblySocket1 socket = hit.GetComponent<AssemblySocket1>();
            if (socket != null && socket.allowedPartID == partID)
            {
                // 🔍 尋找子物件 SnapPoint
                Transform snapPoint = socket.transform.Find("SnapPoint");

                if (snapPoint != null)
                {
                    transform.position = snapPoint.position;
                    transform.rotation = snapPoint.rotation;
                }
                else
                {
                    transform.position = socket.transform.position;
                    transform.rotation = socket.transform.rotation;
                }

                // ✅ 設為子物件、鎖定
                transform.SetParent(socket.transform);
                rb.isKinematic = true;
                rb.useGravity = false;

                Debug.Log($"✅ {name} 成功插入 {socket.name}");
                return;
            }
        }

        Debug.Log($"❌ {name} 沒有找到匹配插槽");
    }
}


