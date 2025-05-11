using UnityEngine;

public class Draggable1 : MonoBehaviour
{
    private Camera cam;
    private bool isDragging = false;
    private float distance;

    void Start()
    {
        // 確保從場景中找到 Camera（不用 Camera.main）
        cam = FindObjectOfType<Camera>();
    }

    void Update()
    {
        if (cam == null) return;

        // 開始拖動
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    isDragging = true;
                    distance = Vector3.Distance(cam.transform.position, transform.position);
                }
            }
        }

        // 拖動中
        if (isDragging && Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 point = ray.GetPoint(distance);
            transform.position = point;
        }

        // 停止拖動
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
}






