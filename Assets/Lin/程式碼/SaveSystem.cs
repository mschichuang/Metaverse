using UnityEngine;
using System.Threading.Tasks;

public class UnlockManager : MonoBehaviour
{
    public int price = 10;
    private bool unlocked = false;

    public bool IsUnlocked() => unlocked;

    public async Task<bool> TryUnlock()
    {
        int diamonds = await SaveSystem.GetDiamonds();

        if (diamonds >= price)
        {
            diamonds -= price;
            await SaveSystem.SetDiamonds(diamonds);
            unlocked = true;
            return true;
        }
        else
        {
            return false;
        }
    }
}

