using UnityEngine;

public class Grabbable : MonoBehaviour
{
    private bool isGrabbed = false;
    private Transform originalParent;
    private Rigidbody rb;

    void Start()
    {
        originalParent = transform.parent; // 記錄原始父物件
        rb = GetComponent<Rigidbody>(); // 取得 Rigidbody
    }

    public void Grab(Transform hand)
    {
        isGrabbed = true;
        transform.SetParent(hand); // 設定為角色的手部
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        rb.isKinematic = true; // 讓物品不受物理影響
    }

    public void Release()
    {
        isGrabbed = false;
        transform.SetParent(originalParent); // 回到原位
        rb.isKinematic = false; // 重新啟用物理效果
    }
}


