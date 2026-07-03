namespace WeatherDemo.Models.OpenMeteo
{
    public enum OpenMeteoWeather
    {
        ClearSky = 0,
        MainlyClear = 1,
        PartlyCloudy = 2,
        Overcast = 3,
        Fog = 45,
        RimeFog = 48,
        LightDrizzle = 51,
        ModerateDrizzle = 53,
        DenseDrizzle = 55,
        LightFreezingDrizzle = 56,
        DenseFreezingDrizzle = 57,
        SlightRain = 61,
        ModerateRain = 63,
        HeavyRain = 65,
        LightFreezingRain = 66,
        HeavyFreezingRain = 67,
        SlightSnowfall = 71,
        ModerateSnowfall = 73,
        HeavySnowfall = 75,
        SnowGrains = 77,
        SlightRainShower = 80,
        ModerateRainShower = 81,
        ViolentRainShower = 82,
        SlightSnowShower = 85,
        HeavySnowShower = 86,
        Thunderstorm = 95,
    }
}
