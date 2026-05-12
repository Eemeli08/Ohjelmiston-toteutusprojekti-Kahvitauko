using Kahvitauko_ohjelma.View;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using Kahvitauko_ohjelma.Controller;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Text.Json;


namespace Kahvitauko_ohjelma
{
    public partial class Mainform : Form
    {
        //    Controller.ProgServices services = new Controller.ProgServices();  luo palvelimen olion, joka sisältää kaikki endpointit.
        public Mainform()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            //services.StartServers();-->  käynnistää palvelimen, kun Form1‑ikkuna avautuu.
            //Näiden avulla ohjelma avaa portin 5001 ja alkaa vastata selaimen pyyntöihin(Time, Weather, Price).
            _ = new Controller.ProgServices().StartServers();

            // Hakee hinnan heti kun ohjelma käynnistyy.
            await FetchAndDisplayPrice();
            timer1.Start();
        }
        private async Task FetchAndDisplayPrice() // Hakee sähkön hinnan palvelimelta ja näyttää sen Hintalbl:ssä, tämä tapahtuu, kun ohjelma käynnistyy.
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string json = await client.GetStringAsync("http://localhost:5000/price/now/");

                    using var doc = JsonDocument.Parse(json);
                    double price = doc.RootElement.GetProperty("PriceNow").GetDouble();
                    string unit = doc.RootElement.GetProperty("Unit").GetString();

                    Hintalbl.Text = $"Price: {price:F2} {unit}";
                }
            }
            catch (Exception ex)
            {
                Hintalbl.Text = "Error: " + ex.Message;
            }
        }
        private async void FetchWeatherButton_Click(object sender, EventArgs e)
        {
            // 1. Haetaan säädata (ProgServices-olion kautta, kuten aiemminkin)
            var data = await new Controller.ProgServices().GetWeatherAsync(dateTimePicker1.Value);

            if (!string.IsNullOrEmpty(data.Error))
            {
                MessageBox.Show(data.Error);
                return;
            }

            // 2. Näytetään perustiedot
            label2.Text = $"Temperature: {data.TempC} °C";
            label3.Text = $"Sunlight: {data.Sunlight} %";
            Tuuli.Text = $"Wind: {data.WindSpeed} m/s";

            // 3. Lasketaan auringon korkeus ja paneelin teho käyttäen uusia metodeja
            double elevation = Controller.ProgServices.GetSolarElevation(dateTimePicker1.Value);

            // Use the instance method on the local SolarPanel (signature: CalculatePower(double elevationDegrees, double sunlightPercent))
            double power = _solarPanel.CalculatePower(elevation, data.Sunlight);

            // 4. Näytetään tulos
            Solarlabel.Text = $"Aurinkopaneeli: {power:F2} kW";
        }
        private SolarPanel _solarPanel = new SolarPanel();
        private async void btnOpenTime_Click(object sender, EventArgs e) // Hakee kellonajan palvelimelta ja näyttää sen label1:ssä, tämä tapahtuu, kun käyttäjä klikkaa "Aika" -nappia.
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string result = await client.GetStringAsync("http://localhost:5000/time/");
                    var data = JsonSerializer.Deserialize<JsonDocument>(result);
                    string time = data.RootElement.GetProperty("CurrentTime").GetString();
                    label1.Text = $"Kellonaika: {time}";
                }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {
            // Tyhjä tapahtumankäsittelijä, joka voidaan poistaa tai käyttää myöhemmin
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // Tyhjä tapahtumankäsittelijä, joka voidaan poistaa tai käyttää myöhemmin
        }

        private void reset_Click(object sender, EventArgs e)
        {
            // Resetataan kaikki labelit oletusteksteihin
            label1.Text = "Aika";
            label2.Text = "Lämpö";
            label3.Text = "Aurinko";
            Tuuli.Text = "Tuuli";
            Hintalbl.Text = "Hinta";
            Solarlabel.Text = "Aurinkopaneeli";
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // Tyhjä tapahtumankäsittelijä, joka voidaan poistaa tai käyttää myöhemmin
        }
        private void richTextBox1_Load(object sender, EventArgs e) // Hakee tiedot tietokannan taulusta ja näyttää ne richTextBox1:ssä, tämä tapahtuu, kun richTextBox1 ladataan, eli siis heti, kun ohjelma käynnistyy. (testausta vaan)
        {
            // Yhdistetään tietokantaan ja haetaan Laite-taulun data(väliaikaista koodia koska testailen)
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Sähkötiedot;Integrated Security=True;Pooling=False;Encrypt=True;Trust Server Certificate=False";
            string query = "SELECT * FROM Sää_Data";

            StringBuilder sb = new StringBuilder();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                sb.Append(reader.GetValue(i).ToString() + "\t");
                            }

                            sb.AppendLine();
                        }

                    }
                }
                richTextBox1.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void richTextBox2_Load(object sender, EventArgs e) // Hakee tiedot tietokannan taulusta ja näyttää ne richTextBox1:ssä, tämä tapahtuu, kun richTextBox1 ladataan, eli siis heti, kun ohjelma käynnistyy. (testausta vaan)
        {
            // Yhdistetään tietokantaan ja haetaan Laite-taulun data(väliaikaista koodia koska testailen)
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Sähkötiedot;Integrated Security=True;Pooling=False;Encrypt=True;Trust Server Certificate=False";
            string query = "SELECT * FROM Sähkö_Data";

            StringBuilder sb = new StringBuilder();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                sb.Append(reader.GetValue(i).ToString() + "\t");
                            }

                            sb.AppendLine();
                        }

                    }
                }
                richTextBox2.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            // Tyhjä tapahtumankäsittelijä, joka voidaan poistaa tai käyttää myöhemmin
        }
        private async void btnTestPrice_Click(object sender, EventArgs e)
        {
            // Testataan FetchAndDisplayPrice-metodia, joka hakee sähkön hinnan ja päivittää Hintalbl:n tekstin,
            // hinta päivittyy ja tulostuu labelille aina kun ohjelma käynnistyy, mutta tällä napilla voi hakea hinnan uudestaan halutessaan.
            await FetchAndDisplayPrice();
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            // Tyhjä tapahtumankäsittelijä, joka voidaan poistaa tai käyttää myöhemmin
        }

        private void btnSave_Click(object sender, EventArgs e) // Tallennetaan säädata tietokantaan
        {
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Sähkötiedot;Integrated Security=True";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Sähkö_Data (Päivä_Aika, hinta_kwh) VALUES (@Päivä_Aika, @hinta_kwh)";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@Päivä_Aika", DateTime.Now);

                    // Puhdistetaan Hintalbl.Text:stä ylimääräiset merkit ja muutetaan desimaalierotin pilkuksi ennen tallennusta
                    string cleanPrice = Hintalbl.Text
                        .Replace("Price:", "")
                        .Replace("snt/kWh", "")
                        .Replace(".", ",")
                        .Trim();

                    cmd.Parameters.AddWithValue("@hinta_kwh", decimal.Parse(cleanPrice));

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e) //Tallentaa sään tiedot tietokantaan, eli siis lämpötilan, auringonvalon ja tuulen nopeuden. Tämä tapahtuu, kun käyttäjä klikkaa "Tallenna sää" -nappia.
        {
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Sähkötiedot;Integrated Security=True";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Sää_Data (Päivä_Aika, Lämpö, Aurinko, Tuuli, Aurinkopaneeli) VALUES (@Päivä_Aika, @Lämpö, @Aurinko, @Tuuli, @Aurinkopaneeli)";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@Päivä_Aika", dateTimePicker1.Value);

                    string cleanTemp = label2.Text.Replace("Temperature:", "").Replace("°C", "").Replace(".", ",").Trim();
                    cmd.Parameters.AddWithValue("@Lämpö", decimal.Parse(cleanTemp));

                    string cleanSun = label3.Text.Replace("Sunlight:", "").Replace("%", "").Replace(".", ",").Trim();
                    cmd.Parameters.AddWithValue("@Aurinko", decimal.Parse(cleanSun));

                    string cleanWind = Tuuli.Text.Replace("Wind:", "").Replace("m/s", "").Replace(".", ",").Trim();
                    cmd.Parameters.AddWithValue("@Tuuli", decimal.Parse(cleanWind));

                    string cleanSolar = Solarlabel.Text.Replace("Aurinkopaneeli:", "").Replace("kW", "").Replace(".", ",").Trim();
                    cmd.Parameters.AddWithValue("@Aurinkopaneeli", decimal.Parse(cleanSolar));

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Weather data saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            // Tallennusvirheen käsittely
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private double GetSolarElevation(DateTime time)
        {
            // Yksinkertainen malli auringon korkeudesta, joka perustuu päivämäärään ja kellonaikaan
            double lat = 62.2; // Jyväskylä
            double lon = 25.7;
            int dayOfYear = time.DayOfYear;
            double hour = time.Hour + time.Minute / 60.0;

            double declination = 23.45 * Math.Sin(Math.PI / 180 * (360.0 / 365 * (dayOfYear - 81)));
            double hourAngle = 15 * (hour - 12);

            double elevation = Math.Asin(
                Math.Sin(lat * Math.PI / 180) * Math.Sin(declination * Math.PI / 180) +
                Math.Cos(lat * Math.PI / 180) * Math.Cos(declination * Math.PI / 180) *
                Math.Cos(hourAngle * Math.PI / 180)
            ) * 180 / Math.PI;

            return Math.Max(0, elevation);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            settingsform settingsForm = new settingsform();
            settingsForm.ShowDialog();

        }
    }
}
public class SolarPanel
{
    // Yksinkertainen malli aurinkopaneelin tehosta, joka perustuu auringon korkeuteen ja pilvisyyteen
    public double MaxPowerKw { get; set; } = 5.0;  // kWp
    public double TiltAngle { get; set; } = 35;    // kallistuskulma
    public double AzimuthAngle { get; set; } = 180; // etelä

    public double CalculatePower(double solarElevationDeg, double sunlightPercent) // solarElevationDeg: Auringon korkeusasteina, sunlightPercent: Pilvisyysprosentti
    {
        // Jos aurinko on horisontin alapuolella, teho on nolla
        if (solarElevationDeg <= 0) return 0;

        double elevationRad = solarElevationDeg * Math.PI / 180;
        double tiltRad = TiltAngle * Math.PI / 180;

        double angleFactor = Math.Max(0, Math.Min(1, Math.Sin(elevationRad + tiltRad)));
        double cloudFactor = sunlightPercent / 100.0;

        return MaxPowerKw * angleFactor * cloudFactor;
    }
}
