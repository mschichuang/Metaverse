using UnityEngine;

namespace Emily.Scripts
{
    public class SubmissionMatrixAnimator : MonoBehaviour
    {
        [Header("Crystal Settings")]
        public Transform crystalRoot;
        public float crystalHoverSpeed = 1.0f;
        public float crystalHoverAmplitude = 0.1f;
        public float crystalRotationSpeed = 20f;

        [Header("Rings Settings")]
        public Transform ringHorizontal;
        public Transform ringTilted;
        public float ringRotationSpeed = 30f;

        [Header("Obelisk Settings")]
        // We'll use arrays to store references to the floating parts
        public Transform[] obeliskTips;
        public Transform[] obeliskLowers;
        public float obeliskHoverSpeed = 0.5f;
        public float obeliskHoverAmplitude = 0.05f;

        [Header("Particles")]
        public Transform[] particles;
        public float particleOrbitSpeed = 10f;

        // Private variables to store initial positions
        private Vector3 startPosCrystal;
        private Vector3[] startPosObeliskTips;
        private Vector3[] startPosObeliskLowers;

        private void Start()
        {
            // Cache initial positions for floating math
            if(crystalRoot) startPosCrystal = crystalRoot.localPosition;

            if(obeliskTips != null)
            {
                startPosObeliskTips = new Vector3[obeliskTips.Length];
                for(int i=0; i<obeliskTips.Length; i++) 
                    if(obeliskTips[i]) startPosObeliskTips[i] = obeliskTips[i].localPosition;
            }

            if(obeliskLowers != null)
            {
                startPosObeliskLowers = new Vector3[obeliskLowers.Length];
                for(int i=0; i<obeliskLowers.Length; i++) 
                    if(obeliskLowers[i]) startPosObeliskLowers[i] = obeliskLowers[i].localPosition;
            }
        }

        private void Update()
        {
            float t = Time.time;

            // 1. Crystal Animation
            if(crystalRoot)
            {
                // Hover
                float yOffset = Mathf.Sin(t * crystalHoverSpeed) * crystalHoverAmplitude;
                crystalRoot.localPosition = startPosCrystal + Vector3.up * yOffset;
                // Rotate
                crystalRoot.Rotate(Vector3.up, crystalRotationSpeed * Time.deltaTime);
            }

            // 2. Rings Animation
            if(ringHorizontal)
                ringHorizontal.Rotate(Vector3.up, ringRotationSpeed * Time.deltaTime); // Clockwise
            
            if(ringTilted)
                 ringTilted.Rotate(Vector3.up, -ringRotationSpeed * 0.8f * Time.deltaTime, Space.Self); // Counter-clockwise, local axis

            // 3. Obelisk Floating (Desynchronized)
            if(obeliskTips != null)
            {
                for(int i=0; i<obeliskTips.Length; i++)
                {
                    if(obeliskTips[i])
                    {
                        // Offset phase by index to create "wave" effect
                        float phase = i * 1.5f; 
                        float yOff = Mathf.Sin(t * obeliskHoverSpeed + phase) * obeliskHoverAmplitude;
                        obeliskTips[i].localPosition = startPosObeliskTips[i] + Vector3.up * yOff;
                    }
                }
            }
            // Make lowers float slightly differently (lagging behind)
            if(obeliskLowers != null)
            {
                for(int i=0; i<obeliskLowers.Length; i++)
                {
                    if(obeliskLowers[i])
                    {
                        float phase = i * 1.5f + 0.5f; // Slight lag
                        float yOff = Mathf.Sin(t * obeliskHoverSpeed + phase) * (obeliskHoverAmplitude * 0.5f); // Less movement
                        obeliskLowers[i].localPosition = startPosObeliskLowers[i] + Vector3.up * yOff;
                    }
                }
            }

            // 4. Particles Orbiting
            if(particles != null)
            {
                // Rotate them as a group would be easier if they had a parent container, 
                // but since they are loose, we can just rotate them around the center manually 
                // OR just rotate them locally if they were offset... 
                // The current builder places them at world-ish positions under root.
                // Let's just make them bobble for now to keep it simple, or rotate around center (0,0,0 local)
                
                // Let's do a simple orbit around Y axis center (0,0,0) of this object
                // We'll compute it by rotating their position vector
                foreach(var p in particles)
                {
                    if(p)
                    {
                        // Rotate position vector around up axis
                        // This moves the particles in a circle around the terminal center
                        p.RotateAround(transform.position, transform.up, particleOrbitSpeed * Time.deltaTime);
                        
                        // Also tumble them
                        p.Rotate(Vector3.one, 30f * Time.deltaTime);
                    }
                }
            }
        }
    }
}
