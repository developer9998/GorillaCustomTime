using GorillaCustomTime.Utilities;
using GorillaInfoWatch.Models;
using GorillaInfoWatch.Models.Attributes;
using GorillaInfoWatch.Models.Widgets;
using System.Linq;
using WeatherType = BetterDayNightManager.WeatherType;

[assembly: InfoWatchCompatible]

namespace GorillaCustomTime.Models
{
    [ShowOnHomeScreen]
    internal class CustomTimeScreen : InfoScreen
    {
        public override string Title => "Custom Time";
        public override string Description => $"Using {Constants.Name} [{Constants.Version}]";

        private int? timeOfDay = null;
        private bool? isRain = null;

        public override void OnScreenLoad()
        {
            NetworkSystem.Instance.OnMultiplayerStarted += SetContent;
            NetworkSystem.Instance.OnReturnedToSinglePlayer += OnRoomLeft;
            ZoneManagement.instance.onZoneChanged += SetContent;
        }

        public override void OnScreenUnload()
        {
            NetworkSystem.Instance.OnMultiplayerStarted -= SetContent;
            NetworkSystem.Instance.OnReturnedToSinglePlayer -= OnRoomLeft;
            ZoneManagement.instance.onZoneChanged -= SetContent;
        }

        public override InfoContent GetContent()
        {
            LineBuilder lines = new();

            if (!Plugin.InModdedRoom)
            {
                lines.Add("Please enter a modded room to use GorillaCustomTime").Skip();
                lines.Add("Your changes will persist if you stay in a modded.");
                return lines;
            }

            int timeOfDay = this.timeOfDay.GetValueOrDefault(TimeUtility.TimeOfDay);
            lines.Add($"Time of Day: {TimeUtility.GetTimeOfDayName(timeOfDay)}", new Widget_SnapSlider(timeOfDay, 0, TimeUtility.TimeManager.dayNightLightmapNames.Length - 1, SetTimeOfDay));

            bool isRain = this.isRain.GetValueOrDefault(TimeUtility.Weather == WeatherType.Raining);
            lines.Add($"Weather: {(isRain ? (ZoneManagement.instance.IsZoneActive(GTZone.mountain) ? "Snow" : "Rain") : "None")}", new Widget_Switch(isRain, SetWeather));

            if (this.timeOfDay.HasValue || this.isRain.HasValue) lines.Skip().Add("Reset", new Widget_PushButton(Reset));

            return lines;
        }

        public void SetTimeOfDay(int value)
        {
            if (!Plugin.InModdedRoom) return;

            timeOfDay = value;
            TimeUtility.SetTimeOfDay(value);

            SetContent();
        }

        public void SetWeather(bool value)
        {
            if (!Plugin.InModdedRoom) return;

            isRain = value;
            TimeUtility.SetWeather(value ? WeatherType.Raining : WeatherType.None);

            SetContent();
        }

        public void Reset()
        {
            timeOfDay = null;
            isRain = null;
            TimeUtility.ResetTimeOfDay();
            TimeUtility.ResetWeather();

            SetContent();
        }

        public void OnRoomLeft()
        {
            timeOfDay = null;
            isRain = null;

            SetContent();
        }
    }
}
