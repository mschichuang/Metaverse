using UnityEngine;

namespace Emily.Scripts
{
    /// <summary>
    /// 傳送門動畫控制器
    /// 創造螺旋上升的能量粒子效果
    /// </summary>
    public class TeleporterAnimator : MonoBehaviour
    {
        [Header("動畫物件參考")]
        public Transform teleportBeam;       // 傳送光束
        public Transform[] energyParticles;  // 能量粒子（多個小球）
        
        [Header("光束旋轉")]
        public float beamRotationSpeed = 15f;
        
        [Header("粒子螺旋動畫")]
        public int particleCount = 6;        // 粒子數量
        public float spiralRadius = 0.5f;    // 螺旋半徑
        public float spiralHeight = 2.5f;    // 螺旋總高度
        public float riseSpeed = 0.8f;       // 上升速度
        public float rotationSpeed = 60f;    // 螺旋旋轉速度
        
        // 粒子進度（0-1）
        private float[] particleProgress;

        void Start()
        {
            // 初始化粒子進度（錯開起始位置）
            if (energyParticles != null && energyParticles.Length > 0)
            {
                particleProgress = new float[energyParticles.Length];
                for (int i = 0; i < energyParticles.Length; i++)
                {
                    particleProgress[i] = (float)i / energyParticles.Length;
                }
            }
        }

        void Update()
        {
            // 1. 光束緩慢旋轉
            if (teleportBeam)
            {
                teleportBeam.Rotate(Vector3.up, beamRotationSpeed * Time.deltaTime, Space.Self);
            }
            
            // 2. 粒子螺旋上升
            AnimateSpiralParticles();
        }
        
        /// <summary>
        /// 粒子沿著螺旋路徑上升
        /// </summary>
        void AnimateSpiralParticles()
        {
            if (energyParticles == null || particleProgress == null) return;
            
            for (int i = 0; i < energyParticles.Length; i++)
            {
                if (energyParticles[i])
                {
                    // 更新進度（循環）
                    particleProgress[i] += riseSpeed * Time.deltaTime;
                    if (particleProgress[i] > 1f) particleProgress[i] -= 1f;
                    
                    float progress = particleProgress[i];
                    
                    // 計算螺旋位置
                    float angle = progress * 360f * 2f + (rotationSpeed * Time.time);
                    float height = progress * spiralHeight;
                    
                    float x = Mathf.Cos(angle * Mathf.Deg2Rad) * spiralRadius;
                    float z = Mathf.Sin(angle * Mathf.Deg2Rad) * spiralRadius;
                    
                    energyParticles[i].localPosition = new Vector3(x, height, z);
                    
                    // 粒子大小隨高度變化（底部小，中間大，頂部消失）
                    float scale = Mathf.Sin(progress * Mathf.PI) * 0.15f;
                    energyParticles[i].localScale = Vector3.one * scale;
                }
            }
        }
    }
}
