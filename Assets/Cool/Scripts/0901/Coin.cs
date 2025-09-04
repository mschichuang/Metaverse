using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1; // 這顆金幣值多少

    private void OnTriggerEnter(Collider other)
    {
        // 判斷玩家名稱是否包含 "Avatar"
        if (other.gameObject.name.Contains("Avatar"))
        {
            // 更新左上角數字
            CoinManager.Instance.AddCoin(coinValue);

            // 金幣消失
            Destroy(gameObject);
        }
    }
}
