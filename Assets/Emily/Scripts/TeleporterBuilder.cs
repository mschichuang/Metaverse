using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Emily.Scripts
{
    public class TeleporterBuilder : MonoBehaviour
    {
        [ContextMenu("Generate Teleporter Pad")]
        public void Generate()
        {
            // 1. Root Object
            GameObject root = new GameObject("TeleporterPad");
            root.transform.position = transform.position;

            // Helper to get shader
            Shader litShader = Shader.Find("Universal Render Pipeline/Lit");
            if (litShader == null) litShader = Shader.Find("Standard");

            // --- MATERIALS ---
            // Dark Metal (Floor)
            Material matMetal = new Material(litShader);
            matMetal.color = new Color(0.2f, 0.2f, 0.25f);
            if (litShader.name.Contains("Universal")) { matMetal.SetFloat("_Metallic", 0.8f); matMetal.SetFloat("_Smoothness", 0.3f); }

            // Glowing Core (Cyan)
            Material matCore = new Material(litShader);
            matCore.color = Color.cyan;
            matCore.EnableKeyword("_EMISSION");
            matCore.SetColor("_EmissionColor", Color.cyan * 2.5f);

            // Pylon Glow (Blue)
            Material matPylonGlow = new Material(litShader);
            matPylonGlow.color = new Color(0, 0.5f, 1f);
            matPylonGlow.EnableKeyword("_EMISSION");
            matPylonGlow.SetColor("_EmissionColor", new Color(0, 0.5f, 1f) * 2.0f);

            // Beam (Transparent)
            Material matBeam = new Material(litShader);
            if (litShader.name.Contains("Universal"))
            {
                // URP Lit Shader Transparency Setup
                matBeam.SetFloat("_Surface", 1); // 1 = Transparent
                matBeam.SetFloat("_Blend", 0);   // 0 = Alpha
                matBeam.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                matBeam.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                matBeam.SetInt("_ZWrite", 0);
                matBeam.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                matBeam.renderQueue = 3000;
                
                // Ensure alpha clipping is off for smooth transparency
                matBeam.SetFloat("_AlphaClip", 0);
                matBeam.DisableKeyword("_ALPHATEST_ON");
            }
            else
            {
                // Standard Shader Fallback
                matBeam.SetFloat("_Mode", 3); // 3 = Transparent
                matBeam.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                matBeam.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                matBeam.SetInt("_ZWrite", 0);
                matBeam.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                matBeam.renderQueue = 3000;
            }
            matBeam.color = new Color(0, 1, 1, 0.15f); // Very faint cyan
            matBeam.EnableKeyword("_EMISSION");
            matBeam.SetColor("_EmissionColor", Color.cyan * 0.3f);


            // --- STRUCTURE ---

            // 2. Main Platform (Base)
            GameObject platform = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            platform.name = "Platform_Base";
            platform.transform.SetParent(root.transform);
            platform.transform.localPosition = new Vector3(0, 0.1f, 0);
            platform.transform.localScale = new Vector3(2.5f, 0.2f, 2.5f); // 2.5m wide
            platform.GetComponent<Renderer>().sharedMaterial = matMetal;

            // 3. Inner Core (Glowing Ring/Center)
            GameObject core = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            core.name = "Power_Core";
            core.transform.SetParent(root.transform);
            core.transform.localPosition = new Vector3(0, 0.11f, 0); // Slightly higher
            core.transform.localScale = new Vector3(1.5f, 0.2f, 1.5f);
            core.GetComponent<Renderer>().sharedMaterial = matCore;

            // 4. Pylons (3 Pillars)
            int pylonCount = 3;
            float radius = 1.1f;
            for (int i = 0; i < pylonCount; i++)
            {
                float angle = i * (360f / pylonCount);
                Vector3 pos = Quaternion.Euler(0, angle, 0) * new Vector3(radius, 0.5f, 0);
                
                // Pylon Body
                GameObject pylon = GameObject.CreatePrimitive(PrimitiveType.Cube);
                pylon.name = $"Pylon_{i}";
                pylon.transform.SetParent(root.transform);
                pylon.transform.localPosition = pos;
                pylon.transform.localScale = new Vector3(0.4f, 1.0f, 0.4f);
                pylon.transform.localRotation = Quaternion.Euler(0, angle, 0);
                pylon.GetComponent<Renderer>().sharedMaterial = matMetal;

                // Pylon Light Strip
                GameObject strip = GameObject.CreatePrimitive(PrimitiveType.Cube);
                strip.name = "Light_Strip";
                strip.transform.SetParent(pylon.transform);
                strip.transform.localPosition = new Vector3(0, 0, -0.45f); // Face inward
                strip.transform.localScale = new Vector3(0.2f, 0.8f, 0.1f);
                strip.GetComponent<Renderer>().sharedMaterial = matPylonGlow;
            }

            // 5. Teleport Beam (Visual Zone)
            GameObject beam = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            beam.name = "Teleport_Beam";
            beam.transform.SetParent(root.transform);
            beam.transform.localPosition = new Vector3(0, 1.5f, 0);
            beam.transform.localScale = new Vector3(1.4f, 1.5f, 1.4f); // Fits inside core
            beam.GetComponent<Renderer>().sharedMaterial = matBeam;
            
            // FIX: DestroyImmediate is banned in some contexts. 
            // Instead of destroying the collider, we just disable it.
            var beamCol = beam.GetComponent<Collider>();
            if (beamCol != null) beamCol.enabled = false;

            // 6. Energy Particles (螺旋上升的能量粒子)
            int particleCount = 6;
            GameObject[] particles = new GameObject[particleCount];
            for (int i = 0; i < particleCount; i++)
            {
                GameObject particle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                particle.name = $"Energy_Particle_{i}";
                particle.transform.SetParent(root.transform);
                particle.transform.localPosition = Vector3.zero; // 動畫會控制位置
                particle.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                particle.GetComponent<Renderer>().sharedMaterial = matCore; // 使用核心的發光材質
                
                // 禁用粒子碰撞器
                var particleCol = particle.GetComponent<Collider>();
                if (particleCol != null) particleCol.enabled = false;
                
                particles[i] = particle;
            }

            // --- ANIMATION SETUP ---
            
            // Add TeleporterAnimator and setup references
            TeleporterAnimator animator = root.AddComponent<TeleporterAnimator>();
            
            // 設定傳送光束
            animator.teleportBeam = beam.transform;
            
            // 設定能量粒子
            animator.energyParticles = new Transform[particleCount];
            for (int i = 0; i < particleCount; i++)
            {
                animator.energyParticles[i] = particles[i].transform;
            }

            // 7. Collider for Interaction (Restricted to Beam area)
            // Beam is at y=1.5, scale y=1.5 (height ~3m), scale x/z=1.4 (radius ~0.7m)
            CapsuleCollider col = root.AddComponent<CapsuleCollider>();
            col.center = new Vector3(0, 1.5f, 0);
            col.radius = 0.7f; // 0.5 * 1.4
            col.height = 3.0f; // 2.0 * 1.5
            col.isTrigger = true;

            // 7. Add SpacePortal component
            SpacePortal portal = root.AddComponent<SpacePortal>();
            // Note: spaceID needs to be set manually in Inspector

            Debug.Log("✓ Teleporter Generated!\n" +
                "✓ 螺旋上升動畫已設定（6個能量粒子）\n" +
                "✓ SpacePortal 已自動掛載\n" +
                "請手動設定 Space ID");
        }
    }
}
