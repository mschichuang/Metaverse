using UnityEngine;

public class AssemblySocket1 : MonoBehaviour
{
    [Tooltip("允許的零件 ID（相同 string 才能組裝）")]
    public string allowedPartID;

    [Tooltip("放入後是否固定為子物件")]
    public bool attachAsChild = true;

    [Tooltip("是否在放入後鎖定位置與旋轉")]
    public bool lockTransform = true;

    // 可視化用
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 0.2f);
    }
}
