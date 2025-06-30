using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    void Start()
    {
        DragAndDropManager1.Register(this.gameObject);
    }
}


