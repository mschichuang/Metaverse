using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    public Transform holdPosition; // 這是玩家拿取物品的位置
    private Grabbable grabbedObject; // 目前拿起的物品

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 按滑鼠左鍵
        {
            if (grabbedObject == null)
            {
                TryPickup();
            }
            else
            {
                DropObject();
            }
        }
    }

    void TryPickup()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 3f))
        {
            Grabbable grabbable = hit.collider.GetComponent<Grabbable>();
            if (grabbable != null)
            {
                grabbedObject = grabbable;
                grabbedObject.Grab(holdPosition);
            }
        }
    }

    void DropObject()
    {
        if (grabbedObject != null)
        {
            grabbedObject.Release();
            grabbedObject = null;
        }
    }
}

