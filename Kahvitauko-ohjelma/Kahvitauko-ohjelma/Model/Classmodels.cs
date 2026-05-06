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
            public int Sunlight { get; set; }
            public double WindSpeed { get; set; }
            public string Error { get; set; }
        }

    }
}
