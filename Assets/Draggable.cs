using UnityEngine;

public class Draggable : MonoBehaviour
{
    public Camera referenceCamera;
    private bool isDragging = false;
    private Vector3 offset;

    void Start()
    {
        if (referenceCamera == null)
        {
            GameObject camObj = GameObject.FindGameObjectWithTag("MainCamera");
            if (camObj != null)
            {
                referenceCamera = camObj.GetComponent<Camera>();
            }
        }
    }

    void OnMouseDown()
    {
        isDragging = true;
        Vector3 mousePosition = referenceCamera.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    void Update()
    {
        if (isDragging && referenceCamera != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = referenceCamera.WorldToScreenPoint(transform.position).z;
            Vector3 worldPosition = referenceCamera.ScreenToWorldPoint(mousePosition);
            transform.position = new Vector3(worldPosition.x, worldPosition.y, transform.position.z) + offset;
        }
    }
}




