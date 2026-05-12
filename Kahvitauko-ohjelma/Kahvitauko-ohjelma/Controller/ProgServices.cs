using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Kahvitauko_ohjelma.Model.Classmodels;

namespace Kahvitauko_ohjelma.Controller
{
    internal class ProgServices
    {
        public async Task StartServers()
        {
            HttpListener listener = new HttpListener();
            //Aika
            listener.Prefixes.Add("http://localhost:5000/time/");
            //Sää
            listener.Prefixes.Add("http://localhost:5000/weather/");
            // Sähkön hinta (nyt)
            listener.Prefixes.Add("http://localhost:5000/price/now/");
            // Sähkön hinta (päivä)
            listener.Prefixes.Add("http://localhost:5000/price/date/");

            try { listener.Start(); }
            catch (Exception ex) { MessageBox.Show("Server Error: " + ex.Message); return; }

            while (true)
            {
                try
                {
                    HttpListenerContext context = await listener.GetContextAsync();
                    HttpListenerResponse response = context.Response;
                    string jsonResponse = "";
                    string path = context.Request.Url.LocalPath;

                    if (path == "/time/") // Aika endpoint
                    {
                        var timeData = new { CurrentTime = DateTime.Now.ToString("HH:mm:ss") };
                        jsonResponse = JsonSerializer.Serialize(timeData);
                    }
                    else if (path == "/weather/") // Sää endpoint
                    {
                        string reqDateStr = context.Request.QueryString["date"];
                        if (string.IsNullOrEmpty(reqDateStr))
                        {
                            jsonResponse = JsonSerializer.Serialize(new { Error = "Missing date parameter." });
                        }
                        else
                        {
                            try
                            {
                                DateTime reqDate = DateTime.Parse(reqDateStr);
                                bool isArchive = reqDate < DateTime.Today.AddDays(-2);

                                string fields = isArchive
                                    ? "temperature_2m_max,wind_speed_10m_max,sunshine_duration"
                                    : "temperature_2m_max,wind_speed_10m_max,cloudcover_mean";

                                var uriBuilder = new UriBuilder();
                                uriBuilder.Scheme = "https";
                                uriBuilder.Host = isArchive ? "archive-api.open-meteo.com" : "api.open-meteo.com";
                                uriBuilder.Path = isArchive ? "v1/archive" : "v1/forecast";
                                uriBuilder.Query = $"latitude=62.24&longitude=25.75&daily={fields}&timezone=auto&start_date={reqDateStr}&end_date={reqDateStr}";

                                using (HttpClient extClient = new HttpClient())
                                {
                                    string apiUrl = uriBuilder.ToString();
                                    string apiRes = await extClient.GetStringAsync(apiUrl);

                                    using var doc = JsonDocument.Parse(apiRes);
                                    var daily = doc.RootElement.GetProperty("daily");

                                    double sunlight;
                                    if (isArchive)
                                    {
                                        // sunshine_duration = sekunteja päivässä, max 86400s = 100%
                                        double sunshine = daily.GetProperty("sunshine_duration").EnumerateArray().First().GetDouble();
                                        sunlight = Math.Round(sunshine / 86400 * 100, 1);
                                    }
                                    else
                                    {
                                        // cloudcover = pilvisyys %, käännetään auringoksi
                                        double cloud = daily.GetProperty("cloudcover_mean").EnumerateArray().First().GetDouble();
                                        sunlight = Math.Round(100 - cloud, 1);
                                    }

                                    var result = new
                                    {
                                        TempC = daily.GetProperty("temperature_2m_max").EnumerateArray().First().GetDouble(),
                                        Sunlight = sunlight,
                                        WindSpeed = daily.GetProperty("wind_speed_10m_max").EnumerateArray().First().GetDouble()
                                    };
                                    jsonResponse = JsonSerializer.Serialize(result);
                                }
                            }
                            catch (Exception ex)
                            {
                                jsonResponse = JsonSerializer.Serialize(new { Error = "Server Error: " + ex.Message });
                            }
                        }
                    }

                    else if (path == "/price/now/") // Sähkön hinta nyt endpoint
                    {
                        try
                        {
                            using (HttpClient client = new HttpClient())
                            {
                                client.DefaultRequestHeaders.Add("x-api-key", "d748defac84d4c48880332961af279ab");
                                string apiUrl = "https://data.fingrid.fi/api/datasets/106/data";
                                string apiResponse = await client.GetStringAsync(apiUrl);

                                using var doc = JsonDocument.Parse(apiResponse);
                                var dataArray = doc.RootElement.GetProperty("data");
                                var latest = dataArray.EnumerateArray().Last();

                                double priceMWh = latest.GetProperty("value").GetDouble();
                                double priceCents = priceMWh / 10.0;

                                var result = new
                                {
                                    PriceNow = priceCents,
                                    Unit = "snt/kWh",
                                    Time = latest.GetProperty("startTime").GetString()
                                };

                                jsonResponse = JsonSerializer.Serialize(result);
                            }
                        }
                        catch (Exception ex)
                        {
                            jsonResponse = JsonSerializer.Serialize(new { Error = ex.Message });
                        }
                    }

                    else if (path == "/price/date") // Sähkön hinta päivälle endpoint
                    {
                        string reqDateStr = context.Request.QueryString["date"];

                        if (string.IsNullOrEmpty(reqDateStr))
                        {
                            jsonResponse = JsonSerializer.Serialize(new { Error = "Missing date parameter." });
                        }
                        else
                        {
                            try
                            {
                                DateTime reqDate = DateTime.Parse(reqDateStr);
                                string start = reqDate.ToString("yyyy-MM-ddT00:00:00Z");
                                string end = reqDate.ToString("yyyy-MM-ddT23:59:59Z");

                                using (HttpClient client = new HttpClient())
                                {
                                    client.DefaultRequestHeaders.Add("x-api-key", "d748defac84d4c48880332961af279ab");

                                    string apiUrl =
                                        $"https://data.fingrid.fi/api/datasets/106/data?startTime={start}&endTime={end}";

                                    string apiResponse = await client.GetStringAsync(apiUrl);

                                    using var doc = JsonDocument.Parse(apiResponse);
                                    var prices = doc.RootElement.EnumerateArray().ToList();

                                    if (prices.Count == 0)
                                    {
                                        jsonResponse = JsonSerializer.Serialize(new { Error = "No price data for this date." });
                                    }
                                    else
                                    {
                                        var hourlyPrices = new List<double>();

                                        foreach (var p in prices)
                                        {
                                            double priceMWh = p.GetProperty("value").GetDouble();
                                            double priceCents = priceMWh / 10.0;
                                            hourlyPrices.Add(priceCents);
                                        }

                                        var result = new
                                        {
                                            Date = reqDateStr,
                                            HourlyPrices = hourlyPrices,
                                            Unit = "snt/kWh"
                                        };

                                        jsonResponse = JsonSerializer.Serialize(result);

                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                jsonResponse = JsonSerializer.Serialize(new { Error = ex.Message });
                            }
                        }
                    }

                        byte[] buffer = Encoding.UTF8.GetBytes(jsonResponse);
                    response.ContentType = "application/json";
                    response.ContentLength64 = buffer.Length;
                    await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                    response.OutputStream.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Server Error: " + ex.Message);
                }
            }
        }

        public async Task<WeatherResponse> GetWeatherAsync(DateTime date)
        {
            // Avoimen meteon API vaatii päivämäärän muodossa YYYY-MM-DD, joten muotoillaan se ennen pyyntöä
            try
            {
                string formattedDate = date.ToString("yyyy-MM-dd");
                using (var client = new HttpClient())
                {
                    string json = await client.GetStringAsync($"http://localhost:5000/weather/?date={formattedDate}");

                    return JsonSerializer.Deserialize<WeatherResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            catch (Exception ex)
            {
                return new WeatherResponse { Error = ex.Message };
            }
        }

        public double GetSolarElevation(DateTime time, double lat, double lon)
        {
            // Päivän numero vuodessa
            int dayOfYear = time.DayOfYear;
            double hour = time.Hour + time.Minute / 60.0;

            // Deklinaatio (auringon kulma päiväntasaajaan)
            double declination = 23.45 * Math.Sin(Math.PI / 180 * (360.0 / 365 * (dayOfYear - 81)));

            // Tuntikulma
            double hourAngle = 15 * (hour - 12);

            // Auringon korkeus
            double elevation = Math.Asin(
                Math.Sin(lat * Math.PI / 180) * Math.Sin(declination * Math.PI / 180) +
                Math.Cos(lat * Math.PI / 180) * Math.Cos(declination * Math.PI / 180) *
                Math.Cos(hourAngle * Math.PI / 180)
            ) * 180 / Math.PI;

            return Math.Max(0, elevation); // ei negatiivinen (yö)
        }

    }
}
