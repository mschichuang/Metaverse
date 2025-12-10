using UnityEngine;

namespace Emily.Scripts
{
    /// <summary>
    /// 全息控制台動畫控制器
    /// 只控制全息螢幕旋轉
    /// </summary>
    public class ConsoleAnimator : MonoBehaviour
    {
        [Header("動畫物件參考")]
        public Transform hologramScreen;     // 全息螢幕
        
        [Header("全息螢幕旋轉")]
        public float screenRotationSpeed = 20f;

        void Update()
        {
            // 全息螢幕持續旋轉
            if (hologramScreen)
            {
                hologramScreen.Rotate(Vector3.up, screenRotationSpeed * Time.deltaTime, Space.Self);
            }
        }
    }
}
