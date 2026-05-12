using System;

namespace Kahvitauko_ohjelma.Controller
{
    public class SolarPanel
    {
        // Simple physical model for panel power in kW
        private const double PanelArea = 1.6; // m^2
        private const double Efficiency = 0.18; // 18%
        private const double PeakIrradianceKwPerM2 = 1.0; // kW/m^2 at peak

        public double CalculatePower(double elevationDegrees, double sunlightPercent)
        {
            if (elevationDegrees <= 0 || sunlightPercent <= 0)
                return 0.0;

            double incidence = Math.Sin(elevationDegrees * Math.PI / 180.0);
            double irradiance = PeakIrradianceKwPerM2 * (sunlightPercent / 100.0) * incidence;
            double powerKw = PanelArea * irradiance * Efficiency;
            return powerKw;
        }
        public static double GetSolarElevation(DateTime time)
        {
            // Yksinkertainen malli auringon korkeudesta (Jyväskylä)
            double lat = 62.2;
            double declination = 23.45 * Math.Sin(Math.PI / 180 * (360.0 / 365 * (time.DayOfYear - 81)));
            double hourAngle = 15 * ((time.Hour + time.Minute / 60.0) - 12);

            double elevation = Math.Asin(
                Math.Sin(lat * Math.PI / 180) * Math.Sin(declination * Math.PI / 180) +
                Math.Cos(lat * Math.PI / 180) * Math.Cos(declination * Math.PI / 180) *
                Math.Cos(hourAngle * Math.PI / 180)
            ) * 180 / Math.PI;

            return Math.Max(0, elevation);
        }

        internal static double CalculatePower(Model.Classmodels.SolarPanelget panel, double solarElevationDeg, double sunlightPercent)
        {
            if (solarElevationDeg <= 0) return 0;

            double elevationRad = solarElevationDeg * Math.PI / 180;
            double tiltRad = panel.TiltAngle * Math.PI / 180;

            double angleFactor = Math.Max(0, Math.Min(1, Math.Sin(elevationRad + tiltRad)));
            double cloudFactor = sunlightPercent / 100.0;

            return panel.MaxPowerKw * angleFactor * cloudFactor;
        }        
    }
}

