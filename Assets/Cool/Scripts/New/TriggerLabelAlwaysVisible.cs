using UnityEngine;

public class TriggerLabelAlwaysVisible : MonoBehaviour
{
    [Header("Label Settings")]
    public string labelText = "觸發點"; // 顯示的文字
    public Vector3 labelOffset = new Vector3(0, 2, 0); // 文字偏移量
    public GameObject labelPrefab; // 用於顯示文字的 Prefab

    private GameObject labelInstance; // 動態生成的文字物件

    private void Start()
    {
        if (labelPrefab == null)
        {
            Debug.LogError("請設定文字顯示的 Prefab！");
            return;
        }

        // 生成文字物件
        labelInstance = Instantiate(labelPrefab, transform.position + labelOffset, Quaternion.identity, transform);
        labelInstance.GetComponent<TextMesh>().text = labelText;
    }
}
