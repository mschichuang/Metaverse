using UnityEngine;

public class Draggable : MonoBehaviour
{
    public Camera referenceCamera;
    private bool isDragging = false;
    private float objectZ;

    void Start()
    {
        if (referenceCamera == null)
        {
            referenceCamera = FindObjectOfType<Camera>();
        }
    }

    void OnMouseDown()
    {
        isDragging = true;
        objectZ = referenceCamera.WorldToScreenPoint(transform.position).z;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    void Update()
    {
        if (isDragging && referenceCamera != null)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = objectZ;
            Vector3 worldPos = referenceCamera.ScreenToWorldPoint(mousePos);
            transform.position = worldPos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ReturnZone"))
        {
            Destroy(gameObject); // 刪除物件，模擬「收回背包」
            // 或者改成：gameObject.SetActive(false);
            // 或者呼叫背包管理器：BackpackManager.Instance.ReturnItem(gameObject);
        }
    }
}





