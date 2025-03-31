using UnityEngine;
using SpatialSys.UnitySDK;

public class SpacePortal : MonoBehaviour
{
    public string targetSpaceSKU;

    public void Teleport()
    {
        SpatialBridge.spaceService.TeleportToSpace(targetSpaceSKU, true);
    }
}