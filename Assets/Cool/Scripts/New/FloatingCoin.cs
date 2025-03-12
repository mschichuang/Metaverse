using UnityEngine;

public class FloatingCoin : MonoBehaviour
{
    public float floatAmplitude = 0.5f;
    public float floatSpeed = 2f;
    public float rotationSpeed = 100f;
    public int coinValue = 10;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CoinManager.AddCoins(coinValue);
            Destroy(gameObject);
        }
    }
}
