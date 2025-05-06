using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab;
    public Camera referenceCamera;

    public void SpawnItem()
    {
        Debug.Log("✅ SpawnItem() 被呼叫了！");

        if (referenceCamera == null)
        {
            Debug.LogError("❌ Reference Camera 未設定！");
            return;
        }

        Vector3 spawnPosition = referenceCamera.transform.position + referenceCamera.transform.forward * 1f;
        GameObject newItem = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
        Debug.Log("✅ 成功生成物件：" + newItem.name);

        Draggable dragScript = newItem.GetComponent<Draggable>();
        if (dragScript == null)
        {
            dragScript = newItem.AddComponent<Draggable>();
        }

        dragScript.referenceCamera = referenceCamera;
        Debug.Log("✅ 成功設置 referenceCamera 給 Draggable");
    }
}



