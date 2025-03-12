using UnityEngine;

public class Move : MonoBehaviour
{
    public float moveSpeed = 3f;  // 移動速度
    public float turnSpeed = 90f; // 轉向速度（度/秒）

    private void Update()
    {
        float moveInput = Input.GetAxis("Vertical");  // W/S 或 ↑/↓ 控制前後
        float turnInput = Input.GetAxis("Horizontal"); // A/D 或 ←/→ 控制左右轉向

        // 移動角色（根據角色面朝的方向）
        transform.Translate(Vector3.forward * moveInput * moveSpeed * Time.deltaTime);

        // 旋轉角色（左右轉向）
        transform.Rotate(Vector3.up * turnInput * turnSpeed * Time.deltaTime);
    }
}
