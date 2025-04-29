using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab;
    public Transform spawnPoint;

    public void SpawnItem()
    {
        Instantiate(itemPrefab, spawnPoint.position, Quaternion.identity);
    }
}
