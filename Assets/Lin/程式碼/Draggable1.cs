using UnityEngine;  // ✅ using 放在最前面！

public class Draggable1 : MonoBehaviour
{
    private Camera cam;
    private bool isDragging = false;
    private float distance;

    void Start()
    {
        // 確保從場景中找到 Camera
        cam = FindObjectOfType<Camera>();
    }

    void Update()
    {
        if (cam == null) return;

        // 點擊開始拖曳
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    isDragging = true;
                    distance = Vector3.Distance(cam.transform.position, transform.position);
                }
            }
        }

        // 拖曳中
        if (isDragging && Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 point = ray.GetPoint(distance);
            transform.position = point;
        }

        // 停止拖曳
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
}




