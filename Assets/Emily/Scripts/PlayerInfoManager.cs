using SpatialSys.UnitySDK;

public static class PlayerInfoManager
{

    public static string GetPlayerGroup()
    {
        return SpatialBridge.actorService.localActor.displayName.Split(' ')[0];
    }

    public static string GetPlayerName()
    {
        return SpatialBridge.actorService.localActor.displayName.Split(' ')[1];
    }
}