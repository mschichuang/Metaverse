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

            // --- MATERIALS (Retail Palette) ---
            
            // 1. Glossy White (Walls/Shelves)
            Material matWhite = new Material(litShader);
            matWhite.name = "Mat_GlossWhite";
            matWhite.color = new Color(0.95f, 0.95f, 0.95f);
            if (litShader.name.Contains("Universal")) { matWhite.SetFloat("_Metallic", 0.0f); matWhite.SetFloat("_Smoothness", 0.9f); }

            // 2. Tech Blue (Accents)
            Material matBlue = new Material(litShader);
            matBlue.name = "Mat_TechBlue";
            matBlue.color = new Color(0.0f, 0.4f, 0.8f);
            if (litShader.name.Contains("Universal")) { matBlue.SetFloat("_Metallic", 0.5f); matBlue.SetFloat("_Smoothness", 0.6f); }

            // 3. Neon Sign (Bright)
            Material matNeon = new Material(litShader);
            matNeon.name = "Mat_NeonSign";
            matNeon.color = new Color(0.0f, 0.8f, 1.0f);
            matNeon.EnableKeyword("_EMISSION");
            matNeon.SetColor("_EmissionColor", new Color(0.0f, 0.6f, 1.0f) * 3.0f);

            // 4. Glass (Windows)
            Material matGlass = new Material(litShader);
            matGlass.name = "Mat_ClearGlass";
            matGlass.color = new Color(0.8f, 0.9f, 1.0f, 0.3f); 
            if (litShader.name.Contains("Universal")) 
            {
                matGlass.SetFloat("_Surface", 1.0f); // Transparent
                matGlass.SetFloat("_Blend", 0.0f); 
                matGlass.SetInt("_ZWrite", 0);
            }

            // 5. Product Box Materials (Variety)
            Material matProdRed = new Material(litShader); matProdRed.color = Color.red;
            Material matProdGreen = new Material(litShader); matProdGreen.color = Color.green;
            Material matProdYellow = new Material(litShader); matProdYellow.color = Color.yellow;


            // --- ARCHITECTURE: CYBER-RETAIL STORE ---
            // Clear identity: Boxy, big glass front, sign on top, shelves inside.

            float width = 7.0f;
            float depth = 5.0f;
            float height = 3.5f;

            // 1. THE BOX STRUCTURE
            // Floor
            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "Shop_Floor";
            floor.transform.SetParent(root.transform);
            floor.transform.localPosition = new Vector3(0, 0.1f, 0);
            floor.transform.localScale = new Vector3(width, 0.2f, depth);
            floor.GetComponent<Renderer>().sharedMaterial = matWhite;

            // Back Wall
            GameObject backWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            backWall.name = "Back_Wall";
            backWall.transform.SetParent(root.transform);
            backWall.transform.localPosition = new Vector3(0, height/2f, depth/2f - 0.25f);
            backWall.transform.localScale = new Vector3(width, height, 0.5f);
            backWall.GetComponent<Renderer>().sharedMaterial = matWhite;

            // Side Walls
            for(int s=-1; s<=1; s+=2)
            {
                GameObject sideWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                sideWall.transform.SetParent(root.transform);
                sideWall.transform.localPosition = new Vector3(s * (width/2f - 0.25f), height/2f, 0);
                sideWall.transform.localScale = new Vector3(0.5f, height, depth);
                sideWall.GetComponent<Renderer>().sharedMaterial = matWhite;
            }

            // Roof
            GameObject roof = GameObject.CreatePrimitive(PrimitiveType.Cube);
            roof.name = "Shop_Roof";
            roof.transform.SetParent(root.transform);
            roof.transform.localPosition = new Vector3(0, height, 0);
            roof.transform.localScale = new Vector3(width + 0.5f, 0.4f, depth + 1.0f); // Overhang front
            roof.GetComponent<Renderer>().sharedMaterial = matWhite;

            // 2. THE STOREFRONT (Glass & Sign)
            // Big Sign Board
            GameObject signBoard = GameObject.CreatePrimitive(PrimitiveType.Cube);
            signBoard.name = "Main_Sign";
            signBoard.transform.SetParent(root.transform);
            signBoard.transform.localPosition = new Vector3(0, height + 0.8f, -depth/2f - 0.5f); // Front edge top
            signBoard.transform.localScale = new Vector3(width, 1.2f, 0.3f);
            signBoard.GetComponent<Renderer>().sharedMaterial = matBlue;

            // "SHOP" Text Simulation (Glowing Strip)
            GameObject signText = GameObject.CreatePrimitive(PrimitiveType.Cube);
            signText.transform.SetParent(signBoard.transform);
            signText.transform.localPosition = new Vector3(0, 0, -0.6f);
            signText.transform.localScale = new Vector3(0.8f, 0.4f, 0.1f);
            signText.GetComponent<Renderer>().sharedMaterial = matNeon;

            // Storefront Glass
            GameObject glassFront = GameObject.CreatePrimitive(PrimitiveType.Cube);
            glassFront.transform.SetParent(root.transform);
            glassFront.transform.localPosition = new Vector3(0, height/2f, -depth/2f + 0.1f);
            glassFront.transform.localScale = new Vector3(width - 1.0f, height - 0.5f, 0.1f); // Leave gap for door
            glassFront.GetComponent<Renderer>().sharedMaterial = matGlass;

            // 3. INTERIOR - COUNTER
            GameObject counter = GameObject.CreatePrimitive(PrimitiveType.Cube);
            counter.name = "Checkout_Counter";
            counter.transform.SetParent(root.transform);
            counter.transform.localPosition = new Vector3(1.5f, 0.8f, 0); // Right side
            counter.transform.localScale = new Vector3(2.0f, 0.9f, 0.8f);
            counter.GetComponent<Renderer>().sharedMaterial = matBlue;

            // Register
            GameObject register = GameObject.CreatePrimitive(PrimitiveType.Cube);
            register.transform.SetParent(counter.transform);
            register.transform.localPosition = new Vector3(0, 0.6f, 0);
            register.transform.localScale = new Vector3(0.4f, 0.3f, 0.4f);
            register.GetComponent<Renderer>().sharedMaterial = matWhite;


            // 4. INTERIOR - SHELVES & PRODUCTS
            // 2 Large Shelves on Back Wall
            for(int x=-1; x<=0; x++) // Left and Center
            {
                GameObject rack = new GameObject($"Shelf_Rack_{x}");
                rack.transform.SetParent(root.transform);
                rack.transform.localPosition = new Vector3((x * 2.0f) - 1.0f, 0, depth/2f - 1.0f); // Back left area

                // Rack Frame
                GameObject frame = GameObject.CreatePrimitive(PrimitiveType.Cube);
                frame.transform.SetParent(rack.transform);
                frame.transform.localPosition = new Vector3(0, 1.5f, 0);
                frame.transform.localScale = new Vector3(1.8f, 3.0f, 0.5f);
                frame.GetComponent<Renderer>().sharedMaterial = matWhite;

                // Shelf Levels
                for(int y=0; y<4; y++)
                {
                    float yPos = 0.5f + (y * 0.7f);
                    GameObject shelf = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    shelf.transform.SetParent(rack.transform);
                    shelf.transform.localPosition = new Vector3(0, yPos, -0.3f);
                    shelf.transform.localScale = new Vector3(1.6f, 0.05f, 0.6f);
                    shelf.GetComponent<Renderer>().sharedMaterial = matBlue;

                    // Products on Shelf
                    for(int p=0; p<3; p++)
                    {
                        GameObject prod = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        prod.name = "Product_Box";
                        prod.transform.SetParent(shelf.transform);
                        prod.transform.localPosition = new Vector3((p*0.5f) - 0.5f, 0.25f, 0);
                        prod.transform.localScale = new Vector3(0.3f, 0.4f, 0.3f);
                        
                        // Random color
                        if (p==0) prod.GetComponent<Renderer>().sharedMaterial = matProdRed;
                        else if (p==1) prod.GetComponent<Renderer>().sharedMaterial = matProdGreen;
                        else prod.GetComponent<Renderer>().sharedMaterial = matProdYellow;
                    }
                }
            }
            
            // 5. DECORATIONS
            // Tech Pillars Front corners
            for(int c=-1; c<=1; c+=2)
            {
                GameObject pillar = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                pillar.transform.SetParent(root.transform);
                pillar.transform.localPosition = new Vector3(c * (width/2f - 0.2f), height/2f, -depth/2f + 0.2f);
                pillar.transform.localScale = new Vector3(0.4f, height/2f, 0.4f);
                pillar.GetComponent<Renderer>().sharedMaterial = matBlue;
            }


            Debug.Log("✓ Generated 'Cyber-Retail Store' - Unmistakably a shop!");
        }
    }
}
