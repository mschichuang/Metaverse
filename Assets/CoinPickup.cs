using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int coinValue = 10; // 這顆金幣的價值

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup();
        }
    }

    private void OnMouseDown()
    {
        // 點擊金幣也可以撿（方便滑鼠/平板測試）
        Pickup();
    }

    private void Pickup()
    {
        if (PlayerWallet.Instance != null)
        {
            PlayerWallet.Instance.AddCoins(coinValue);
            Debug.Log($"撿到金幣 +{coinValue}");
        }

        Destroy(gameObject); // 撿完刪除
    }
}

