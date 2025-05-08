using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab;
    [SerializeField] private Camera referenceCamera1;  // 相機1
    [SerializeField] private Camera referenceCamera2;  // 相機2

    public void SpawnItem(bool useCamera1)
    {
        Debug.Log("SpawnItem() 被呼叫了！");

        // 根據需要選擇相機
        Camera selectedCamera = useCamera1 ? referenceCamera1 : referenceCamera2;

        if (selectedCamera == null)
        {
            Debug.LogError("❌ Reference Camera 未設定！");
            return;
        }

        Vector3 spawnPosition = selectedCamera.transform.position + selectedCamera.transform.forward * 1f;
        GameObject newItem = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);

        // 確保 Draggable 元件存在
        Draggable dragScript = newItem.GetComponent<Draggable>();
        if (dragScript == null)
        {
            dragScript = newItem.AddComponent<Draggable>();
        }

        // 使用選擇的相機
        dragScript.SetReferenceCamera(selectedCamera);
    }
}








