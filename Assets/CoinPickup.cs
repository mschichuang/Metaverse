using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int value = 10;  // 金幣數值

    private void OnTriggerEnter(Collider other)
    {
        // 確認碰到的物件是玩家
        if (other.CompareTag("Player"))
        {
            Debug.Log("玩家撿到金幣！ +" + value);

            // 增加金幣
            PlayerWallet.Instance.AddCoins(value);

            // 銷毀金幣
            Destroy(gameObject);
        }
    }
}

