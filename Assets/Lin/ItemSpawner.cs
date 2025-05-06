using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab;
    public Camera referenceCamera;
    void Start()
    {
        if (referenceCamera == null)
        {
            Debug.LogError("❌ Reference Camera 未設定！");
        }
    }

    public void SpawnItem()
    {
        if (referenceCamera == null)
        {
            Debug.LogError("❌ Reference Camera 未設定！");
            return;
        }

        // 生成在攝影機前方 1 公尺處
        Vector3 spawnPosition = referenceCamera.transform.position + referenceCamera.transform.forward * 1f;

        GameObject newItem = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);

        // 指定 Draggable 腳本
        Draggable dragScript = newItem.GetComponent<Draggable>();
        if (dragScript == null)
        {
            dragScript = newItem.AddComponent<Draggable>();
        }

        dragScript.referenceCamera = referenceCamera;
    }
}
