using UnityEngine;
using UnityEngine.UI;

public class WallNavigator : MonoBehaviour
{
    public GameObject[] pages;
    private int currentIndex = 0;

    public void ShowPage(int index)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == index);
        }
        currentIndex = index;
    }

    public void NextPage()
    {
        int next = (currentIndex + 1) % pages.Length;
        ShowPage(next);
    }

    public void PreviousPage()
    {
        int prev = (currentIndex - 1 + pages.Length) % pages.Length;
        ShowPage(prev);
    }

    void Start()
    {
        ShowPage(0);
    }
}

