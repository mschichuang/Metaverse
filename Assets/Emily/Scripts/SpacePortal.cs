using UnityEngine;
using SpatialSys.UnitySDK;

public class SpacePortal : MonoBehaviour
{
    public string spaceID;

    public void Teleport()
    {
        SpatialBridge.spaceService.TeleportToSpace(spaceID, true);
    }
}