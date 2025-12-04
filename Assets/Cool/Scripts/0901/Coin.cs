using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotateSpeed = 90f;
    // 這裡會顯示你在 Inspector 設定的 2000
    public int coinValue = 1; 

    void Update()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }

    // 我們把這個函式改成 public，讓 Spatial 的元件可以呼叫它
    public void Collect()
    {
        // 呼叫管理器加錢
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.AddCoin(coinValue);
        }
        
        // 銷毀金幣
        Destroy(gameObject);
    }
}