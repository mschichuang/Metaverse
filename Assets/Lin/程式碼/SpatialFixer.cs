using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpatialAutoFixer : MonoBehaviour
{
    private static bool alreadyFixed = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void AutoFix()
    {
        if (alreadyFixed) return;
        alreadyFixed = true;

        // 1. 移除場景中所有的 AudioListener（只保留一個也可）
        AudioListener[] listeners = Object.FindObjectsOfType<AudioListener>();
        foreach (AudioListener listener in listeners)
        {
            Debug.Log($"[AutoFix] Removed AudioListener from {listener.gameObject.name}");
            Object.Destroy(listener);
        }

        // 2. 建立 Entrance Point（需要加上 Spatial SDK 的 EntrancePoint component）
        if (GameObject.Find("EntrancePoint") == null)
        {
            GameObject entrance = new GameObject("EntrancePoint");
            entrance.transform.position = Vector3.zero;

            // 嘗試加入 Spatial SDK 的 EntrancePoint Component
            var entranceType = System.Type.GetType("SpatialSys.UnitySDK.EntrancePoint, SpatialCreatorToolkit.Runtime");
            if (entranceType != null)
            {
                entrance.AddComponent(entranceType);
                Debug.Log("[AutoFix] EntrancePoint created with required component.");
            }
            else
            {
                Debug.LogWarning("[AutoFix] EntrancePoint component type not found. Make sure Spatial SDK is installed.");
            }
        }
    }
}

