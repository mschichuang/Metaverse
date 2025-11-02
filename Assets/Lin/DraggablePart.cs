using UnityEngine;

public class DraggablePart : MonoBehaviour
{
    [Tooltip("這個零件的 ID（需與 Socket 的 allowedPartID 相同才可插入）")]
    public string partID;

    [Tooltip("組裝點位置（對應到插槽的中心）")]
    public Transform anchorPoint;

    void Start()
    {
        // 把這個物件註冊給拖曳系統
        DragAndDropManager1.Register(gameObject);
    }
}

