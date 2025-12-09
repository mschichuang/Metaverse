using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Emily.Scripts
{
    public class ShopBuilder : MonoBehaviour
    {
        [ContextMenu("Generate Shop")]
        public void Generate()
        {
            // 1. Root Object
            GameObject root = new GameObject("Shop");
            root.transform.position = transform.position;

            // Helper to get shader
            Shader litShader = Shader.Find("Universal Render Pipeline/Lit");
            if (litShader == null) litShader = Shader.Find("Standard");

            // --- MATERIALS (Premium Palette) ---
            
            // Porcelain White (Armor Shells)
            Material matPorcelain = new Material(litShader);
            matPorcelain.color = new Color(0.96f, 0.96f, 0.98f);
            if (litShader.name.Contains("Universal")) 
            { 
                matPorcelain.SetFloat("_Metallic", 0.15f); 
                matPorcelain.SetFloat("_Smoothness", 0.95f);
            }

            // Champagne Gold (Luxury Trims)
            Material matGold = new Material(litShader);
            matGold.color = new Color(0.88f, 0.78f, 0.5f);
            if (litShader.name.Contains("Universal")) 
            { 
                matGold.SetFloat("_Metallic", 1.0f); 
                matGold.SetFloat("_Smoothness", 0.65f);
            }

            // Deep Space Glass (Core Structure)
            Material matGlass = new Material(litShader);
            matGlass.color = new Color(0.02f, 0.02f, 0.05f, 0.9f);
            if (litShader.name.Contains("Universal"))
            {
                matGlass.SetFloat("_Metallic", 0.8f);
                matGlass.SetFloat("_Smoothness", 0.98f);
            }

            // Quantum Blue (Active Neon)
            Material matNeonBlue = new Material(litShader);
            matNeonBlue.color = new Color(0.0f, 0.9f, 1.0f);
            matNeonBlue.EnableKeyword("_EMISSION");
            matNeonBlue.SetColor("_EmissionColor", new Color(0.0f, 0.6f, 1.0f) * 5.0f);

            // Amber (Warm Accent Neon)
            Material matNeonAmber = new Material(litShader);
            matNeonAmber.color = new Color(1.0f, 0.6f, 0.0f);
            matNeonAmber.EnableKeyword("_EMISSION");
            matNeonAmber.SetColor("_EmissionColor", new Color(1.0f, 0.4f, 0.0f) * 4.0f);

            // Dark Carbon (Base/Details)
            Material matCarbon = new Material(litShader);
            matCarbon.color = new Color(0.12f, 0.12f, 0.12f);
            if (litShader.name.Contains("Universal")) 
            { 
                matCarbon.SetFloat("_Metallic", 0.5f); 
                matCarbon.SetFloat("_Smoothness", 0.4f);
            }


            // --- ARCHITECTURE: CYBER-PAVILION ---

            // 1. HEXAGONAL STEPPED BASE (Approximated with scaled cylinders/cubes)
            // Bottom Tier
            GameObject baseTier1 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            baseTier1.name = "Base_Tier_1";
            baseTier1.transform.SetParent(root.transform);
            baseTier1.transform.localPosition = new Vector3(0, 0.1f, 0);
            baseTier1.transform.localScale = new Vector3(5.5f, 0.2f, 5.5f);
            baseTier1.GetComponent<Renderer>().sharedMaterial = matCarbon;

            // Middle Tier (Gold Ring)
            GameObject baseTier2 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            baseTier2.name = "Base_Tier_2";
            baseTier2.transform.SetParent(root.transform);
            baseTier2.transform.localPosition = new Vector3(0, 0.25f, 0);
            baseTier2.transform.localScale = new Vector3(5.0f, 0.1f, 5.0f);
            baseTier2.GetComponent<Renderer>().sharedMaterial = matGold;

            // Top Tier (Porcelain Platform)
            GameObject baseTier3 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            baseTier3.name = "Base_Tier_3";
            baseTier3.transform.SetParent(root.transform);
            baseTier3.transform.localPosition = new Vector3(0, 0.35f, 0);
            baseTier3.transform.localScale = new Vector3(4.5f, 0.1f, 4.5f);
            baseTier3.GetComponent<Renderer>().sharedMaterial = matPorcelain;


            // 2. INNER GLASS SANCTUM (The darker core)
            GameObject glassCore = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            glassCore.name = "Glass_Sanctum";
            glassCore.transform.SetParent(root.transform);
            glassCore.transform.localPosition = new Vector3(0, 1.8f, 0);
            glassCore.transform.localScale = new Vector3(3.5f, 3.0f, 3.5f); 
            glassCore.GetComponent<Renderer>().sharedMaterial = matGlass;

            // 3. FLOATING ARMOR SHELLS (White panels wrapping the glass)
            // Since we can't easily bend cubes, we use thin partial cylinders or arranged cubes.
            // Let's use 4 main pillars/shells around the corners to create a "Clover" shape look.
            for(int i=0; i<4; i++)
            {
                float angle = 45 + (i * 90);
                float rad = angle * Mathf.Deg2Rad;
                Vector3 pos = new Vector3(Mathf.Sin(rad) * 1.8f, 1.8f, Mathf.Cos(rad) * 1.8f);
                Quaternion rot = Quaternion.Euler(0, angle, 0);

                GameObject shell = GameObject.CreatePrimitive(PrimitiveType.Cube);
                shell.name = $"Armor_Shell_{i}";
                shell.transform.SetParent(root.transform);
                shell.transform.localPosition = pos;
                shell.transform.localRotation = rot;
                shell.transform.localScale = new Vector3(1.5f, 3.2f, 0.4f); // Tall curved-looking panels
                shell.GetComponent<Renderer>().sharedMaterial = matPorcelain;

                // Gold Inlay
                GameObject shellGold = GameObject.CreatePrimitive(PrimitiveType.Cube);
                shellGold.transform.SetParent(shell.transform);
                shellGold.transform.localPosition = new Vector3(0, 0, 0.6f); // Slightly out
                shellGold.transform.localScale = new Vector3(0.2f, 0.9f, 0.1f);
                shellGold.GetComponent<Renderer>().sharedMaterial = matGold;

                // Neon Vent
                GameObject shellNeon = GameObject.CreatePrimitive(PrimitiveType.Cube);
                shellNeon.transform.SetParent(shell.transform);
                shellNeon.transform.localPosition = new Vector3(0, 0.8f, 0.55f);
                shellNeon.transform.localScale = new Vector3(0.8f, 0.05f, 0.1f);
                shellNeon.GetComponent<Renderer>().sharedMaterial = matNeonBlue;
            }


            // 4. GRAND ENTRANCE PORTICO (Front)
            GameObject entranceRoof = GameObject.CreatePrimitive(PrimitiveType.Cube);
            entranceRoof.name = "Portico_Roof";
            entranceRoof.transform.SetParent(root.transform);
            entranceRoof.transform.localPosition = new Vector3(0, 2.6f, -1.8f);
            entranceRoof.transform.localScale = new Vector3(3.0f, 0.2f, 1.5f);
            entranceRoof.GetComponent<Renderer>().sharedMaterial = matPorcelain;

            // Light Pillars (Columns)
            for (int k = -1; k <= 1; k += 2)
            {
                GameObject col = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                col.name = "Portico_Column";
                col.transform.SetParent(root.transform);
                col.transform.localPosition = new Vector3(k * 1.2f, 1.5f, -2.2f);
                col.transform.localScale = new Vector3(0.15f, 1.5f, 0.15f);
                col.GetComponent<Renderer>().sharedMaterial = matGold;

                // Glowing Top/Bottom rings
                GameObject ringTop = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                ringTop.transform.SetParent(col.transform);
                ringTop.transform.localPosition = new Vector3(0, 0.9f, 0);
                ringTop.transform.localScale = new Vector3(1.2f, 0.1f, 1.2f);
                ringTop.GetComponent<Renderer>().sharedMaterial = matNeonBlue;
            }

            // Holographic Carpet (Entrance Path)
            GameObject carpet = GameObject.CreatePrimitive(PrimitiveType.Cube);
            carpet.name = "Holo_Carpet";
            carpet.transform.SetParent(root.transform);
            carpet.transform.localPosition = new Vector3(0, 0.41f, -2.0f);
            carpet.transform.localScale = new Vector3(1.8f, 0.01f, 2.0f);
            carpet.GetComponent<Renderer>().sharedMaterial = matNeonBlue;


            // 5. CROWN STRUCTURE (Roof)
            // Central Dome
            GameObject dome = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            dome.name = "Crown_Dome";
            dome.transform.SetParent(root.transform);
            dome.transform.localPosition = new Vector3(0, 3.4f, 0);
            dome.transform.localScale = new Vector3(3.0f, 0.8f, 3.0f);
            dome.GetComponent<Renderer>().sharedMaterial = matGlass;

            // Floating Halo Rings
            GameObject ring1 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            ring1.name = "Halo_Ring_Large";
            ring1.transform.SetParent(root.transform);
            ring1.transform.localPosition = new Vector3(0, 3.6f, 0);
            ring1.transform.localScale = new Vector3(4.2f, 0.05f, 4.2f);
            ring1.GetComponent<Renderer>().sharedMaterial = matGold;

            GameObject ring2 = GameObject.CreatePrimitive(PrimitiveType.Cylinder); // Inner neon
            ring2.name = "Halo_Ring_Neon";
            ring2.transform.SetParent(root.transform);
            ring2.transform.localPosition = new Vector3(0, 3.6f, 0);
            ring2.transform.localScale = new Vector3(4.0f, 0.06f, 4.0f);
            ring2.GetComponent<Renderer>().sharedMaterial = matNeonBlue;

            // Roof Antenna / Spire
            GameObject spire = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            spire.name = "Energy_Spire";
            spire.transform.SetParent(root.transform);
            spire.transform.localPosition = new Vector3(0, 4.2f, 0);
            spire.transform.localScale = new Vector3(0.1f, 1.0f, 0.1f);
            spire.GetComponent<Renderer>().sharedMaterial = matNeonAmber;


            // 6. INTERNAL ILLUSION (Store Shelf Hints)
            // Simple blocks inside the glass to hint at products
            for(int j=-1; j<=1; j+=2)
            {
                GameObject shelf = GameObject.CreatePrimitive(PrimitiveType.Cube);
                shelf.name = "Internal_Shelf_Shadow";
                shelf.transform.SetParent(glassCore.transform);
                shelf.transform.localPosition = new Vector3(j * 0.3f, 0, 0);
                shelf.transform.localScale = new Vector3(0.1f, 0.8f, 0.6f);
                shelf.GetComponent<Renderer>().sharedMaterial = matCarbon;
            }

            // 7. EXTERIOR SIGNAGE (Holographic Blade)
            GameObject bladeSign = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bladeSign.name = "Blade_Sign";
            bladeSign.transform.SetParent(root.transform);
            bladeSign.transform.localPosition = new Vector3(2.2f, 2.5f, -1.0f);
            bladeSign.transform.localRotation = Quaternion.Euler(0, 0, -15);
            bladeSign.transform.localScale = new Vector3(0.1f, 1.5f, 0.8f);
            bladeSign.GetComponent<Renderer>().sharedMaterial = matPorcelain;

            GameObject bladeGlow = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bladeGlow.transform.SetParent(bladeSign.transform);
            bladeGlow.transform.localPosition = new Vector3(0.6f, 0, 0);
            bladeGlow.transform.localScale = new Vector3(0.1f, 0.9f, 0.9f);
            bladeGlow.GetComponent<Renderer>().sharedMaterial = matNeonAmber;


            Debug.Log("✓ Ultimate Cyber-Pavilion Shop Generated!");
        }
    }
}
