using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace Kahvitauko_ohjelma
{
    public partial class Form1 : Form
    {
        //    Controller.ProgServices services = new Controller.ProgServices();  luo palvelimen olion, joka sisältää kaikki endpointit.
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //services.StartServers();-->  käynnistää palvelimen, kun Form1‑ikkuna avautuu.
            //Näiden avulla ohjelma avaa portin 5001 ja alkaa vastata selaimen pyyntöihin(Time, Weather, Price).
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
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void richTextBox1_Load(object sender, EventArgs e)
        {
            // Yhdistetään tietokantaan ja haetaan Laite-taulun data(väliaikaista koodia koska testailen)
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Sähkötiedot;Integrated Security=True;Pooling=False;Encrypt=True;Trust Server Certificate=False";
            string query = "SELECT * FROM Laite";

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

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
