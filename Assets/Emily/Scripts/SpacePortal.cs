using UnityEngine;
using SpatialSys.UnitySDK;

public class SpacePortal : MonoBehaviour
{
    public string spaceID;

    public void Teleport()
    {
        // publish順序：組裝區 -> 測驗區 -> 導覽區
        SpatialBridge.spaceService.TeleportToSpace(spaceID, true); // url的space-???
    }
}