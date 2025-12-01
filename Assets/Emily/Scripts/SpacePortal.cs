using UnityEngine;
using SpatialSys.UnitySDK;

public class SpacePortal : MonoBehaviour
{
    public string spaceID;

    public void Teleport()
    {
        // publish順序：組裝區 -> 測驗區 -> 導覽區
        SpatialBridge.spaceService.TeleportToSpace(spaceID, true); 
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check name for "(Local)" which is standard for Spatial local avatars
        // This handles cases where SDK avatar reference might be null or not yet linked
        var root = other.transform.root;
        if (root.name.Contains("(Local)") || other.name.Contains("(Local)"))
        {
            Teleport();
        }
    }
}