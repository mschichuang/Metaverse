using UnityEngine;

public class Draggable1 : MonoBehaviour
{
    private Camera cam;
    private bool isDragging = false;
    private float distanceToCamera;

    void Start()
    {
        cam = FindObjectOfType<Camera>();
    }

    void Update()
    {
        if (cam == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    isDragging = true;
                    distanceToCamera = Vector3.Distance(cam.transform.position, transform.position);
                }
            }
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 point = ray.GetPoint(distanceToCamera);
            transform.position = point;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
