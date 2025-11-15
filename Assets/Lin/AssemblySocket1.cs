using UnityEngine;

public class AssemblySocket1 : MonoBehaviour
{
    [Tooltip("������s�� ID�]�ۦP string �~��ոˡ^")]
    public string allowedPartID;

    [Tooltip("��J��O�_�T�w���l����")]
    public bool attachAsChild = true;

    [Tooltip("�O�_�b��J����w��m�P����")]
    public bool lockTransform = true;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 0.2f);
    }
}
