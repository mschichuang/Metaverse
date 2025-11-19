using UnityEngine;

public class AssemblySocket1 : MonoBehaviour
{
    public string allowedPartID;
    public bool attachAsChild = true;
    public bool lockTransform = true;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 0.2f);
    }
}
