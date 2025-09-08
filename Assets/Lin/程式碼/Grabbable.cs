using UnityEngine;

public class Grabbable : MonoBehaviour
{
    private Rigidbody rb;
    private bool isBeingHeld = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Grab()
    {
        isBeingHeld = true;
        rb.useGravity = false;
        rb.drag = 10f;
    }

    public void Release()
    {
        isBeingHeld = false;
        rb.useGravity = true;
        rb.drag = 0f;
    }

    public void MoveTo(Vector3 position)
    {
        if (isBeingHeld)
        {
            Vector3 direction = (position - transform.position);
            rb.velocity = direction * 15f; // 可調整移動速度
        }
    }
}



