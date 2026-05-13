using System;
using System.Collections.Generic;
using System.Text;

namespace Kahvitauko_ohjelma.Model
{
    internal class Classmodels
    {
        public class WeatherResponse
        {
            public double TempC { get; set; }
            public double Sunlight { get; set; }
            public double WindSpeed { get; set; }
            public string? Error { get; set; }

        }

        public class Weather
        {
            public double MaxPowerKw { get; set; } = 5.0;  // kWp
            public double TiltAngle { get; set; } = 35;    // kallistuskulma
            public double AzimuthAngle { get; set; } = 180; // etelä

        }

        public class SolarPanel
        {
            public double MaxPowerKw { get; set; } = 5.0;  // Varmista nimi tästä
            public double TiltAngle { get; set; } = 35;
            public double AzimuthAngle { get; set; } = 180;
        }

        public class SolarPanelget : Panel
        {
            // Nämä pitää olla täällä, jotta ProgServices voi lukea ne
            public double MaxPowerKw { get; set; } = 5.0;
            public double TiltAngle { get; set; } = 35;
            public double AzimuthAngle { get; set; } = 180;
        }


    }
}
