using UnityEngine;

public class Draggable : MonoBehaviour
{
    [SerializeField] private Camera referenceCamera;  // 輸入的相機

    // 提供只讀屬性給其他類別讀取用（不開放直接改）
    public Camera ReferenceCamera => referenceCamera;

    // 提供設定相機的方法
    public void SetReferenceCamera(Camera cam)
    {
        referenceCamera = cam;
    }

    void Start()
    {
        // 如果 referenceCamera 還是 null，顯示錯誤
        if (referenceCamera == null)
        {
            Debug.LogError("❌ ReferenceCamera 尚未設定！");
        }
    }

    void Update()
    {
        if (referenceCamera == null)
        {
            Debug.LogError("❌ ReferenceCamera 尚未設定！");
            return;
        }

        if (!referenceCamera.gameObject.activeInHierarchy)
        {
            Debug.LogError("❌ ReferenceCamera 存在但被關閉！");
            return;
        }

        // 👉 實際的拖曳邏輯可以寫在這裡
    }
}







