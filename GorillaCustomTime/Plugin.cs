using BepInEx;
using GorillaCustomTime.Utilities;
using Utilla.Attributes;

namespace GorillaCustomTime
{
    [BepInDependency("dev.gorillainfowatch")]
    [BepInDependency("org.legoandmars.gorillatag.utilla"), ModdedGamemode]
    [BepInPlugin(Constants.GUID, Constants.Name, Constants.Version)]
    internal class Plugin : BaseUnityPlugin
    {
        public static bool InModdedRoom;

        [ModdedGamemodeJoin]
        public void OnJoin()
        {
            InModdedRoom = true;
        }

        [ModdedGamemodeLeave]
        public void OnLeave()
        {
            InModdedRoom = false;
            TimeUtility.ResetTimeOfDay();
            TimeUtility.ResetWeather();
        }
    }
}
