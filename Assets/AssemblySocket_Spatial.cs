using UnityEngine;

public class AssemblySocket_Spatial : MonoBehaviour
{
    public string allowedPartID;
    public Transform snapPoint;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 0.2f);

        if (snapPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(snapPoint.position, 0.05f);
        }
    }

}

