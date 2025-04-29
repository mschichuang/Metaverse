using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    private bool isDragging = false;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void OnMouseDown()
    {
        isDragging = true;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 2f; // ¶ZÂ÷Äá¼v¾÷ªº¶ZÂ÷
            Vector3 objPosition = cam.ScreenToWorldPoint(mousePosition);
            transform.position = objPosition;
        }
    }
}

