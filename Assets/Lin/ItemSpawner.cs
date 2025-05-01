using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab;          // 拖入你要生成的物品 Prefab
    public float spawnDistance = 2f;       // 距離攝影機的距離
    public Camera referenceCamera;         // 拖入主攝影機

    public void SpawnItem()
    {
        if (referenceCamera == null)
        {
            Debug.LogError("請在 ItemSpawner 指定 referenceCamera");
            return;
        }

        // 在攝影機正前方生成物品
        Vector3 spawnPosition = referenceCamera.transform.position + referenceCamera.transform.forward * spawnDistance;
        GameObject newItem = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);

        // 嘗試加上 Draggable 腳本，並設定攝影機參考
        Draggable dragScript = newItem.GetComponent<Draggable>();
        if (dragScript == null)
        {
            dragScript = newItem.AddComponent<Draggable>();
        }
        dragScript.referenceCamera = referenceCamera;
    }
}
