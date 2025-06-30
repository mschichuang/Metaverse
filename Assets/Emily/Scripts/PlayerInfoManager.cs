using SpatialSys.UnitySDK;

public static class PlayerInfoManager
{
    public const string Url = "https://script.google.com/macros/s/AKfycbyQD56ArfGkOuYfa-RRqYFPbSDLbSdsU98UWw86XBcjPaQ4NJ9GhegNnocDrX5hdlfZ/exec";

    public static string GetPlayerName()
    {
        return SpatialBridge.actorService.localActor.displayName.Split(' ')[1];
    }
}