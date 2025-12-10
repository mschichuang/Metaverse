using UnityEngine;
using SpatialSys.UnitySDK;

/// <summary>
/// 隱藏 Spatial 內建 UI (背包、商店)
/// 每個場景掛載到任意物件上
/// </summary>
public class HideSpatialDefaultUI : MonoBehaviour
{
    private void Start()
    {
        SpatialBridge.coreGUIService.SetCoreGUIEnabled(SpatialCoreGUITypeFlags.Backpack, false);
        SpatialBridge.coreGUIService.SetCoreGUIEnabled(SpatialCoreGUITypeFlags.Shop, false);
    }
}

