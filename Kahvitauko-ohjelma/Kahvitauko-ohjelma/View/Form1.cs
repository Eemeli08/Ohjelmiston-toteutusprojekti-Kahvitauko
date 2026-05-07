using System;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Kahvitauko_ohjelma
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Molemmat palvelimet ylös porttiin 5000
            _ = new Controller.ProgServices().StartServers();
        }



        private async void FetchWeatherButton_Click(object sender, EventArgs e)
        {
            var data = await new Controller.ProgServices().GetWeatherAsync(dateTimePicker1.Value);

            if (!string.IsNullOrEmpty(data.Error))
            {
                MessageBox.Show(data.Error);
                return;
            }

            label2.Text = $"Temperature: {data.TempC} °C";
            label3.Text = $"Sunlight: {data.Sunlight} %";
            Tuuli.Text = $"Wind: {data.WindSpeed} m/s";
        }


        private async void btnOpenTime_Click(object sender, EventArgs e)
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

        // Tyhjät tapahtumankäsittelijät, jotka voidaan poistaa tai käyttää myöhemmin
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void reset_Click(object sender, EventArgs e)
        {
            // Resetataan kaikki labelit oletusteksteihin
            label1.Text = "Aika";
            label2.Text = "Lämpö";
            label3.Text = "Aurinko";
            Tuuli.Text = "Tuuli";
            label4.Text = "pitäs olla database tulostettuna";
        }
    }
}
