using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Emily.Scripts
{
    public class ConsoleBuilder : MonoBehaviour
    {
        [ContextMenu("Generate Console")]
        public void Generate()
        {
            // 1. Root Object
            GameObject root = new GameObject("HolographicConsole");
            root.transform.position = transform.position;
            root.transform.rotation = Quaternion.Euler(0, 180, 0); // 朝向前方

            // Helper to get shader
            Shader litShader = Shader.Find("Universal Render Pipeline/Lit");
            if (litShader == null) litShader = Shader.Find("Standard");

            // --- MATERIALS ---
            // Dark Metal
            Material matMetal = new Material(litShader);
            matMetal.color = new Color(0.15f, 0.15f, 0.15f);
            if (litShader.name.Contains("Universal")) { matMetal.SetFloat("_Metallic", 0.9f); matMetal.SetFloat("_Smoothness", 0.5f); }
            
            // Caution Yellow
            Material matYellow = new Material(litShader);
            matYellow.color = new Color(0.8f, 0.7f, 0.0f);
            
            // Glowing Blue
            Material matGlowBlue = new Material(litShader);
            matGlowBlue.color = Color.cyan;
            matGlowBlue.EnableKeyword("_EMISSION");
            matGlowBlue.SetColor("_EmissionColor", Color.cyan * 2.0f);

            // Glowing Red (Button)
            Material matGlowRed = new Material(litShader);
            matGlowRed.color = Color.red;
            matGlowRed.EnableKeyword("_EMISSION");
            matGlowRed.SetColor("_EmissionColor", Color.red * 2.0f);

            // Hologram Transparent
            Material matHolo = new Material(litShader);
            if (litShader.name.Contains("Universal"))
            {
                matHolo.SetFloat("_Surface", 1); // Transparent
                matHolo.SetFloat("_Blend", 0);
                matHolo.SetInt("_ZWrite", 0);
                matHolo.renderQueue = 3000;
            }
            else
            {
                matHolo.SetFloat("_Mode", 3);
            }
            matHolo.color = new Color(0, 1, 1, 0.3f);
            matHolo.EnableKeyword("_EMISSION");
            matHolo.SetColor("_EmissionColor", Color.cyan * 0.5f);


            // --- STRUCTURE ---

            // 2. Base Tier 1 (Bottom Plate)
            GameObject base1 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            base1.name = "Base_Plate";
            base1.transform.SetParent(root.transform);
            base1.transform.localPosition = new Vector3(0, 0.1f, 0);
            base1.transform.localScale = new Vector3(1.0f, 0.2f, 1.0f);
            base1.GetComponent<Renderer>().sharedMaterial = matMetal;

            // 3. Base Tier 2 (Caution Ring)
            GameObject base2 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            base2.name = "Base_Ring";
            base2.transform.SetParent(root.transform);
            base2.transform.localPosition = new Vector3(0, 0.25f, 0);
            base2.transform.localScale = new Vector3(0.9f, 0.1f, 0.9f);
            base2.GetComponent<Renderer>().sharedMaterial = matYellow;

            // 4. Main Pillar (Cube)
            GameObject pillar = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pillar.name = "Pillar_Main";
            pillar.transform.SetParent(root.transform);
            pillar.transform.localPosition = new Vector3(0, 0.8f, 0);
            pillar.transform.localScale = new Vector3(0.5f, 1.2f, 0.5f);
            pillar.GetComponent<Renderer>().sharedMaterial = matMetal;

            // 5. Tech Strips (Glowing accents on pillar sides)
            for (int i = 0; i < 4; i++)
            {
                GameObject strip = GameObject.CreatePrimitive(PrimitiveType.Cube);
                strip.name = $"Tech_Strip_{i}";
                strip.transform.SetParent(pillar.transform);
                // Position on 4 corners
                float offset = 0.51f; // Slightly outside the pillar
                if (i == 0) strip.transform.localPosition = new Vector3(offset, 0, 0);
                if (i == 1) strip.transform.localPosition = new Vector3(-offset, 0, 0);
                if (i == 2) strip.transform.localPosition = new Vector3(0, 0, offset);
                if (i == 3) strip.transform.localPosition = new Vector3(0, 0, -offset);
                
                strip.transform.localScale = new Vector3(0.1f, 0.8f, 0.1f);
                strip.GetComponent<Renderer>().sharedMaterial = matGlowBlue;
            }

            // 6. Console Head (Slanted Cube)
            GameObject head = GameObject.CreatePrimitive(PrimitiveType.Cube);
            head.name = "Console_Head";
            head.transform.SetParent(root.transform);
            head.transform.localPosition = new Vector3(0, 1.5f, 0.1f);
            head.transform.localScale = new Vector3(0.8f, 0.2f, 0.7f);
            head.transform.localRotation = Quaternion.Euler(20, 0, 0);
            head.GetComponent<Renderer>().sharedMaterial = matMetal;

            // 7. Projector Lens (Sphere on top)
            GameObject lens = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            lens.name = "Projector_Lens";
            lens.transform.SetParent(head.transform);
            lens.transform.localPosition = new Vector3(0, 0.5f, 0.2f); // Top of head
            lens.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            lens.GetComponent<Renderer>().sharedMaterial = matGlowBlue;

            // 8. The RED BUTTON (Cylinder)
            GameObject btn = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            btn.name = "Start_Button";
            btn.transform.SetParent(head.transform);
            btn.transform.localPosition = new Vector3(0, 0.5f, -0.2f); // Lower part of head
            btn.transform.localScale = new Vector3(0.25f, 0.2f, 0.25f);
            btn.transform.localRotation = Quaternion.identity;
            btn.GetComponent<Renderer>().sharedMaterial = matGlowRed;

            // 9. Hologram (Thin Cube for double-sided visibility)
            GameObject holo = GameObject.CreatePrimitive(PrimitiveType.Cube);
            holo.name = "Hologram_Screen";
            holo.transform.SetParent(root.transform);
            holo.transform.localPosition = new Vector3(0, 2.2f, 0.1f);
            // Make it wide but very thin
            holo.transform.localScale = new Vector3(0.8f, 0.5f, 0.02f);
            holo.GetComponent<Renderer>().sharedMaterial = matHolo;
            
            // --- ANIMATION SETUP ---
            
            // 10. Add ConsoleAnimator and setup references
            ConsoleAnimator animator = root.AddComponent<ConsoleAnimator>();
            
            // 只設定全息螢幕（旋轉動畫）
            animator.hologramScreen = holo.transform;

            // 11. Add Interaction Point
            GameObject interactPoint = new GameObject("InteractPoint");
            interactPoint.transform.SetParent(root.transform);
            interactPoint.transform.localPosition = new Vector3(0, 1.5f, 0);
            
            // Add SpatialInteractable
            SpatialSys.UnitySDK.SpatialInteractable interactable = interactPoint.AddComponent<SpatialSys.UnitySDK.SpatialInteractable>();
            interactable.interactText = "測驗";
            interactable.iconType = SpatialSys.UnitySDK.SpatialInteractable.IconType.Weapon;

            // 12. Collider for Interaction
            SphereCollider col = interactPoint.AddComponent<SphereCollider>();
            col.center = Vector3.zero;
            col.radius = 1.2f;

            Debug.Log("✓ Holographic Console Generated!\n" +
                "✓ 全息螢幕旋轉動畫已設定\n" +
                "✓ SpatialInteractable 已設定（測驗）\n" +
                "請手動設定 On Interact Event → QuizManager.StartQuiz()");
        }
    }
}
