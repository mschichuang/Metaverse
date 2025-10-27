using UnityEngine;

public class UnlockManager : MonoBehaviour
{
    public int price = 10;  // 這個物件的價格
    private bool unlocked = false;

    public bool IsUnlocked() => unlocked;

    // 嘗試解鎖
    public bool TryUnlock()
    {
        int diamonds = PlayerPrefs.GetInt("Diamonds", 0);

        if (diamonds >= price)
        {
            diamonds -= price;
            PlayerPrefs.SetInt("Diamonds", diamonds);
            unlocked = true;
            return true;
        }
        else
        {
            return false;
        }
    }
}
