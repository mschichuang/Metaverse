using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    public Transform holdPosition; // �o�O���a�������~����m
    private Grabbable grabbedObject; // �ثe���_�����~

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���ƹ�����
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
        // if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 3f))
        // {
        //     Grabbable grabbable = hit.collider.GetComponent<Grabbable>();
        //     if (grabbable != null)
        //     {
        //         grabbedObject = grabbable;
        //         grabbedObject.Grab(holdPosition);
        //     }
        // }
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

