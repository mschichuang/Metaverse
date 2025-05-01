using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab;
    public Transform spawnPoint;
    public Camera referenceCamera;

    public void SpawnItem()
    {
        GameObject newItem = Instantiate(itemPrefab, spawnPoint.position, Quaternion.identity);

        // 嘗試加上 Draggable 腳本並指定 referenceCamera
        Draggable dragScript = newItem.GetComponent<Draggable>();
        if (dragScript == null)
        {
            dragScript = newItem.AddComponent<Draggable>();
        }
        dragScript.referenceCamera = referenceCamera;
    }
}
