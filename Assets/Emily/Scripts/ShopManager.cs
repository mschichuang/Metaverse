using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject[] pages;

    public void ShowPage(int index)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == index);
        }
    }
}