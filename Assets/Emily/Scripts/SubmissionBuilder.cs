using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Emily.Scripts
{
    public class SubmissionBuilder : MonoBehaviour
    {
        [ContextMenu("Generate Submission Matrix")]
        public void Generate()
        {
            // 1. Root Object
            GameObject root = new GameObject("SubmissionMatrix");
            root.transform.position = transform.position;

            // Helper to get shader
            Shader litShader = Shader.Find("Universal Render Pipeline/Lit");
            if (litShader == null) litShader = Shader.Find("Standard");

            // --- LUXURY TECH MATERIALS ---
            
            // Porcelain White (Body)
            Material matBody = new Material(litShader);
            matBody.color = new Color(0.95f, 0.95f, 0.96f); // Almost white
            if (litShader.name.Contains("Universal")) 
            { 
                matBody.SetFloat("_Metallic", 0.1f); 
                matBody.SetFloat("_Smoothness", 0.9f); // Glossy ceramic look
            }

            // Brushed Gold (Accents)
            Material matGold = new Material(litShader);
            matGold.color = new Color(0.8f, 0.7f, 0.4f);
            if (litShader.name.Contains("Universal")) 
            { 
                matGold.SetFloat("_Metallic", 1.0f); 
                matGold.SetFloat("_Smoothness", 0.85f); 
            }

            // Deep Void (Contrast)
            Material matVoid = new Material(litShader);
            matVoid.color = new Color(0.08f, 0.08f, 0.1f);
            if (litShader.name.Contains("Universal"))
            {
                matVoid.SetFloat("_Metallic", 0.5f);
                matVoid.SetFloat("_Smoothness", 0.4f);
            }

            // Holographic Blue (Interface)
            Material matHolo = new Material(litShader);
            matHolo.color = new Color(0.0f, 0.8f, 1.0f, 0.4f);
            matHolo.EnableKeyword("_EMISSION");
            matHolo.SetColor("_EmissionColor", new Color(0.0f, 0.4f, 1.0f) * 4.0f);
            if (litShader.name.Contains("Universal"))
            {
                matHolo.SetFloat("_Surface", 1); // Transparent
                matHolo.SetFloat("_Blend", 0);
                matHolo.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                matHolo.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                matHolo.SetInt("_ZWrite", 0);
                matHolo.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                matHolo.renderQueue = 3000;
            }
            else
            {
                matHolo.SetFloat("_Mode", 3);
                matHolo.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                matHolo.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                matHolo.SetInt("_ZWrite", 0);
                matHolo.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                matHolo.renderQueue = 3000;
            }

            // Crystal Core (Ethereal)
            Material matCrystal = new Material(litShader);
            matCrystal.color = new Color(0.6f, 1.0f, 1.0f, 0.8f);
            matCrystal.EnableKeyword("_EMISSION");
            matCrystal.SetColor("_EmissionColor", new Color(0.6f, 1.0f, 1.0f) * 2.0f);
             if (litShader.name.Contains("Universal"))
            {
                matCrystal.SetFloat("_Surface", 1);
                matCrystal.SetFloat("_Blend", 0);
                matCrystal.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                matCrystal.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                matCrystal.SetInt("_ZWrite", 0);
                matCrystal.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                matCrystal.renderQueue = 3000;
                matCrystal.SetFloat("_Smoothness", 0.95f);
            }
            else
            {
                matCrystal.SetFloat("_Mode", 3);
                matCrystal.renderQueue = 3000;
            }


            // --- SLEEK GEOMETRY ---

            // 1. The Pedestal (Stacked Rings)
            // Bottom Ring (Dark)
            GameObject baseDark = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            baseDark.name = "Base_Dark";
            baseDark.transform.SetParent(root.transform);
            baseDark.transform.localPosition = new Vector3(0, 0.05f, 0);
            baseDark.transform.localScale = new Vector3(2.2f, 0.1f, 2.2f);
            baseDark.GetComponent<Renderer>().sharedMaterial = matVoid;

            // Main White Ring
            GameObject baseWhite = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            baseWhite.name = "Base_White";
            baseWhite.transform.SetParent(root.transform);
            baseWhite.transform.localPosition = new Vector3(0, 0.15f, 0);
            baseWhite.transform.localScale = new Vector3(1.8f, 0.15f, 1.8f);
            baseWhite.GetComponent<Renderer>().sharedMaterial = matBody;

            // Gold Trim Ring
            GameObject baseGold = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            baseGold.name = "Base_Gold_Trim";
            baseGold.transform.SetParent(root.transform);
            baseGold.transform.localPosition = new Vector3(0, 0.23f, 0);
            baseGold.transform.localScale = new Vector3(1.85f, 0.02f, 1.85f);
            baseGold.GetComponent<Renderer>().sharedMaterial = matGold;

            // 2. Verified Zone Marker (Holographic Floor)
            GameObject holoFloor = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            holoFloor.name = "Holo_Zone";
            holoFloor.transform.SetParent(root.transform);
            holoFloor.transform.localPosition = new Vector3(0, 0.235f, 0);
            holoFloor.transform.localScale = new Vector3(1.4f, 0.01f, 1.4f);
            holoFloor.GetComponent<Renderer>().sharedMaterial = matHolo;

            // 3. Floating Obelisks (3 Pillars)
            int pCount = 3; 
            float pRadius = 1.0f;
            for(int i=0; i<pCount; i++)
            {
                float angle = i * (360f / pCount);
                Vector3 pos = Quaternion.Euler(0, angle, 0) * new Vector3(pRadius, 0, 0);
                
                // Floating Lower Segment
                GameObject pStats = GameObject.CreatePrimitive(PrimitiveType.Cube);
                pStats.name = $"Obelisk_{i}_Lower";
                pStats.transform.SetParent(root.transform);
                pStats.transform.localPosition = pos + new Vector3(0, 0.8f, 0);
                pStats.transform.localScale = new Vector3(0.15f, 0.8f, 0.15f);
                pStats.transform.localRotation = Quaternion.Euler(15f, angle, 0); // Tilt inward slightly
                pStats.GetComponent<Renderer>().sharedMaterial = matBody;

                // Gold Accent Line
                GameObject pGold = GameObject.CreatePrimitive(PrimitiveType.Cube);
                pGold.name = $"Obelisk_{i}_Gold";
                pGold.transform.SetParent(pStats.transform);
                pGold.transform.localPosition = new Vector3(0, 0, 0.55f); // Outer face
                pGold.transform.localScale = new Vector3(0.5f, 1.02f, 0.1f);
                pGold.GetComponent<Renderer>().sharedMaterial = matGold;

                // Floating Upper Tip (Disconnected)
                GameObject pTip = GameObject.CreatePrimitive(PrimitiveType.Cube);
                pTip.name = $"Obelisk_{i}_Tip";
                pTip.transform.SetParent(root.transform);
                pTip.transform.localPosition = pos + new Vector3(0, 1.5f, 0) + (pos.normalized * -0.1f); // Slightly clearer
                pTip.transform.localScale = new Vector3(0.1f, 0.3f, 0.1f);
                pTip.transform.localRotation = Quaternion.Euler(15f, angle, 0);
                pTip.GetComponent<Renderer>().sharedMaterial = matGold;
            }

            // 4. Central Artifact (The "Ascension" Crystal)
            // We'll simulate a diamond shape using two pyramids (or a rotated cube for simplicity)
            GameObject crystal = GameObject.CreatePrimitive(PrimitiveType.Cube);
            crystal.name = "Ascension_Crystal";
            crystal.transform.SetParent(root.transform);
            crystal.transform.localPosition = new Vector3(0, 1.4f, 0);
            crystal.transform.localScale = new Vector3(0.6f, 0.9f, 0.6f);
            // Rotate to stand on corner
            crystal.transform.localRotation = Quaternion.Euler(45, 45, 45); 
            crystal.GetComponent<Renderer>().sharedMaterial = matCrystal;

            // Inner Solid Core (To give the crystal depth)
            GameObject crystalCore = GameObject.CreatePrimitive(PrimitiveType.Cube);
            crystalCore.name = "Crystal_Core";
            crystalCore.transform.SetParent(crystal.transform);
            crystalCore.transform.localPosition = Vector3.zero;
            crystalCore.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            crystalCore.transform.localRotation = Quaternion.identity;
            crystalCore.GetComponent<Renderer>().sharedMaterial = matHolo;


            // 5. Orbital Data Rings (Holographic Interface)
            // Horizontal Ring
            GameObject ringH = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            ringH.name = "Ring_Horizontal";
            ringH.transform.SetParent(root.transform);
            ringH.transform.localPosition = new Vector3(0, 1.4f, 0);
            ringH.transform.localScale = new Vector3(1.4f, 0.02f, 1.4f);
            ringH.GetComponent<Renderer>().sharedMaterial = matHolo;
            
            // Tilted Ring
            GameObject ringT = GameObject.CreatePrimitive(PrimitiveType.Cylinder); // Cylinder as ring
            ringT.name = "Ring_Tilted";
            ringT.transform.SetParent(root.transform);
            ringT.transform.localPosition = new Vector3(0, 1.4f, 0);
            ringT.transform.localScale = new Vector3(1.2f, 0.02f, 1.2f);
            ringT.transform.localRotation = Quaternion.Euler(30, 0, 0);
            ringT.GetComponent<Renderer>().sharedMaterial = matHolo;

            // Make rings hollow-ish by using a second smaller cylinder to "mask" if we were doing CSG, 
            // but for simple primitives, we just rely on surface transparency or create thin torus via code?
            // Since we can't create Torus primitive easily, we stick to flat cylinders (disks) as "force fields".
            // To make it look like a ring, we can use a texture... but we don't have textures.
            // Alternative: Use 4 thin cubes to form a square frame, or just accept the "Disk" look as a "Data Plane".
            // Let's keep the Disks, they look like "scanners".

            // 6. Floating Particles (Small Cubes frozen in ascent)
            for(int i=0; i<8; i++)
            {
                GameObject part = GameObject.CreatePrimitive(PrimitiveType.Cube);
                part.name = $"Data_Bit_{i}";
                part.transform.SetParent(root.transform);
                // Random-ish positions
                float angle = i * 45f;
                float dist = 0.5f + (i % 2) * 0.3f;
                float height = 0.8f + (i * 0.15f);
                Vector3 pos = Quaternion.Euler(0, angle, 0) * new Vector3(dist, height, 0);
                
                part.transform.localPosition = pos;
                part.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                part.transform.localRotation = Quaternion.Euler(Random.value*360, Random.value*360, Random.value*360);
                part.GetComponent<Renderer>().sharedMaterial = matGold;
            }

            // The following line assumes 'shield' GameObject exists and is defined elsewhere in the full context.
            // If 'shield' is not defined, this line will cause a compilation error.
            // It is included here faithfully as per the user's instruction.
            // var shieldCol = shield.GetComponent<Collider>();
            // if (shieldCol != null) shieldCol.enabled = false;

            // 11. Add Animation Script & Setup References
            AscensionTerminalAnimator animator = root.AddComponent<AscensionTerminalAnimator>();
            
            // Crystal
            Transform crystalTrans = root.transform.Find("Ascension_Crystal");
            if(crystalTrans) animator.crystalRoot = crystalTrans;

            // Rings
            Transform ringHTrans = root.transform.Find("Ring_Horizontal");
            if(ringHTrans) animator.ringHorizontal = ringHTrans;
            
            Transform ringTTrans = root.transform.Find("Ring_Tilted");
            if(ringTTrans) animator.ringTilted = ringTTrans;

            // Obelisks
            animator.obeliskTips = new Transform[pCount];
            animator.obeliskLowers = new Transform[pCount];
            for(int i=0; i<pCount; i++)
            {
                var tip = root.transform.Find($"Obelisk_{i}_Tip");
                if(tip) animator.obeliskTips[i] = tip;

                var lower = root.transform.Find($"Obelisk_{i}_Lower");
                if(lower) animator.obeliskLowers[i] = lower;
            }

            // Particles
            animator.particles = new Transform[8]; // Matches loop count 8
            for(int i=0; i<8; i++)
            {
                var p = root.transform.Find($"Data_Bit_{i}");
                if(p) animator.particles[i] = p;
            }

            Debug.Log("✓ Sleek Ascension Terminal (Animated) Generated! Add Spatial Interactable component for interaction.");
        }
    }
}
