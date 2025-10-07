using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotateSpeed = 90f;
    public int coinValue = 1;

    void Update()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }
}