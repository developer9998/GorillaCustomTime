using UnityEngine;
using WeatherType = BetterDayNightManager.WeatherType;

namespace GorillaCustomTime.Utilities
{
    public static class TimeUtility
    {
        public static BetterDayNightManager TimeManager => BetterDayNightManager.instance;
        public static int TimeOfDay => TimeManager.currentTimeIndex;
        public static WeatherType Weather => TimeManager.CurrentWeather();

        public static void SetTimeSetting(TimeSettings setting)
        {
            TimeManager.currentSetting = setting;
        }

        public static void SetTimeOfDay(int index)
        {
            index = Mathf.Clamp(index, 0, TimeManager.timeOfDayRange.Length);
            TimeManager.SetTimeOfDay(index);
            TimeManager.SetOverrideIndex(index);
        }

        public static string GetTimeOfDayName(int index)
        {
            index = Mathf.Clamp(index, 0, TimeManager.timeOfDayRange.Length - 1);
            return TimeManager.dayNightLightmapNames[index];
        }

        public static void UpdateTimeOfDay()
        {
            TimeManager.lastTimeChecked -= TimeManager.currentTimestep;
            TimeManager.UpdateTimeOfDay();
        }

        public static void ResetTimeOfDay()
        {
            SetTimeSetting(TimeSettings.Normal);
            UpdateTimeOfDay();
        }

        public static void SetWeather(WeatherType weather)
        {
            TimeManager.SetFixedWeather(weather);
        }

        public static void ResetWeather()
        {
            TimeManager.ClearFixedWeather();
        }
    }
}
