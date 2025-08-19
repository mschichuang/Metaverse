using System.Collections.Generic;
using UnityEngine;

public class PurchaseHistoryManager : MonoBehaviour
{
    private HashSet<string> purchasedCategories = new HashSet<string>();

    public bool HasPurchasedCategory(string category)
    {
        return purchasedCategories.Contains(category);
    }

    public void AddPurchasedCategory(string category)
    {
        purchasedCategories.Add(category);
    }
}
