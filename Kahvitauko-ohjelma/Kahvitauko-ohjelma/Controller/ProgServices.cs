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
            //Sää(Ilman auringon valoa)
            listener.Prefixes.Add("http://localhost:5000/weather/");

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

                    if (path == "/time/")
                    {
                        var timeData = new { CurrentTime = DateTime.Now.ToString("HH:mm:ss") };
                        jsonResponse = JsonSerializer.Serialize(timeData);
                    }
                    else if (path == "/weather/")
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
                                // hakee toissapäivän, koska avoimen meteon arkistopalvelu ei tarjoa dataa tuoreemmista päivistä
                                bool isArchive = reqDate < DateTime.Today.AddDays(-2);

                                var uriBuilder = new UriBuilder();
                                uriBuilder.Scheme = "https";
                                uriBuilder.Host = isArchive ? "archive-api.open-meteo.com" : "api.open-meteo.com";
                                uriBuilder.Path = isArchive ? "v1/archive" : "v1/forecast";
                                uriBuilder.Query = $"latitude=62.24&longitude=25.75&daily=temperature_2m_max,wind_speed_10m_max&timezone=auto&start_date={reqDateStr}&end_date={reqDateStr}";

                                using (HttpClient extClient = new HttpClient())
                                {
                                    string apiUrl = uriBuilder.ToString();
                                    string apiRes = await extClient.GetStringAsync(apiUrl);

                                    using var doc = JsonDocument.Parse(apiRes);
                                    var daily = doc.RootElement.GetProperty("daily");

                                    var result = new
                                    {
                                        TempC = daily.GetProperty("temperature_2m_max").EnumerateArray().First().GetDouble(),
                                        Sunlight = 75, // Kovakoodattu auringonvalon määrä, koska avoimen meteon API ei tarjoa suoraa tietoa auringonvalosta
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

    }
}
