using UnityEngine;
using System.Collections.Generic;
using SpatialSys.UnitySDK;

public class DragAndDropManager1 : MonoBehaviour
{
    private static List<GameObject> draggableObjects = new List<GameObject>();
    private List<GameObject> selectedObjects = new List<GameObject>();
    private float distanceFromCamera;

    public static void Register(GameObject obj)
    {
        if (!draggableObjects.Contains(obj))
        {
            draggableObjects.Add(obj);
        }
    }

    void Update()
    {
        // 滑鼠按下 → 檢查是否點中任何可拖曳物件
        if (Input.GetMouseButtonDown(0))
        {
            selectedObjects.Clear();

            Ray ray = SpatialBridge.cameraService.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (draggableObjects.Contains(hit.transform.gameObject))
                {
                    selectedObjects.AddRange(draggableObjects);
                    distanceFromCamera = hit.distance;
                }
            }
        }

        // 拖曳所有選到的物件
        if (selectedObjects.Count > 0 && Input.GetMouseButton(0))
        {
            Ray ray = SpatialBridge.cameraService.ScreenPointToRay(Input.mousePosition);
            Vector3 targetPos = ray.GetPoint(distanceFromCamera);

            foreach (var obj in selectedObjects)
            {
                obj.transform.position = targetPos;
            }
        }

        // 放開時清除
        if (Input.GetMouseButtonUp(0))
        {
            selectedObjects.Clear();
        }
    }
}



