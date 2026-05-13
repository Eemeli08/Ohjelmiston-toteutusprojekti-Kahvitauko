using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kahvitauko_ohjelma.View
{
    public partial class settingsform : Form
    {
        public settingsform()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e) // Tömö metodi tallentaa kaikki tiedot tietokantaan, jos kaikki kentät on täytetty, muuten se näyttää varoituksen käyttäjälle.
                                                                     // Tallennus tapahtuu transaktion avulla, joka varmistaa että kaikki tiedot tallentuu onnistuneesti tai ei lainkaan, jos jokin menee pieleen.
        {
            // Ottaa lämmitytiedot, auton tiedot ja sähkösopimustiedot ja tallentaa ne tietokantaan
            string lammitystapa = comboBox1.SelectedItem?.ToString();
            string eristystaso = comboBox3.SelectedItem?.ToString();
            decimal henkilomaara = numericUpDown1.Value;
            decimal aurinkopaneelinMaxteho = numericUpDown2.Value;
            decimal aurinkopaneelinAsKuma = numericUpDown3.Value;
            decimal akunKapasiteetti = numericUpDown4.Value;

            string autontyyppi = comboBox2.SelectedItem?.ToString();
            decimal akunKoko = numericUpDown5.Value;

            decimal siirtomaksu = numericUpDown6.Value;
            decimal siirto = numericUpDown7.Value;
            decimal käyttömaksu = numericUpDown8.Value;
            decimal käyttö = numericUpDown9.Value;

            string kodinkone = textBox1.Text;
            decimal kodinkoneenTeho = numericUpDown10.Value;

            if(

                string.IsNullOrEmpty(lammitystapa) || string.IsNullOrEmpty(eristystaso) ||
                henkilomaara <= 0 || aurinkopaneelinMaxteho <= 0 ||
                aurinkopaneelinAsKuma <= 0 || akunKapasiteetti <= 0 || siirtomaksu <= 0 || siirto <= 0 || käyttömaksu <= 0 || käyttö <= 0 || kodinkoneenTeho <= 0 ||
                string.IsNullOrEmpty(autontyyppi)
                )
            {
                MessageBox.Show("Please make a selection in all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString =
                 "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Sähkötiedot;" +
                 "Integrated Security=True;Pooling=False;Encrypt=False;TrustServerCertificate=True;";

            // SQL query joka lisää tiedot Lämmitys-tauluun, Auto_Tyyppi-tauluun ja Sähkösopimus-tauluun
            string query = "INSERT INTO Lämmitys (Lämmitystapa, Eristystapa, Henkilömäärä, Aurinkopaneelin_maxteho, Aurinkopaneelin_as_kulma, Akun_kapasiteetti) VALUES (@Lämmitystapa, @Eristystapa, @Henkilömäärä, @Aurinkopaneelin_maxteho, @Aurinkopaneelin_as_kulma, @Akun_kapasiteetti)";

            string queryTable2 = "INSERT INTO Auto_Tyyppi (Tyyppi, Akun_koko) VALUES (@Tyyppi, @Akun_koko)";

            string queryTable3 = "INSERT INTO Sähkösopimus (Siirtomaksu, Siirto, Käyttömaksu, Käyttö) VALUES (@Siirtomaksu, @Siirto, @Käyttömaksu, @Käyttö)";

            string queryTable4 = "INSERT INTO Laite (Nimi, Max_Teho) VALUES (@Nimi, @Max_Teho)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 4. Execute the first insert command
                        using (SqlCommand command1 = new SqlCommand(query, connection, transaction))
                        {
                            command1.Parameters.AddWithValue("@Lämmitystapa", lammitystapa);
                            command1.Parameters.AddWithValue("@Eristystapa", eristystaso);
                            command1.Parameters.AddWithValue("@Henkilömäärä", henkilomaara);
                            command1.Parameters.AddWithValue("@Aurinkopaneelin_maxteho", aurinkopaneelinMaxteho);
                            command1.Parameters.AddWithValue("@Aurinkopaneelin_as_kulma", aurinkopaneelinAsKuma);
                            command1.Parameters.AddWithValue("@Akun_kapasiteetti", akunKapasiteetti);

                            await command1.ExecuteNonQueryAsync();
                        }

                        using (SqlCommand command2 = new SqlCommand(queryTable2, connection, transaction))
                        {
                            command2.Parameters.AddWithValue("@Tyyppi", autontyyppi);
                            command2.Parameters.AddWithValue("@Akun_koko", akunKoko);

                            await command2.ExecuteNonQueryAsync();
                        }

                        using (SqlCommand command3 = new SqlCommand(queryTable3, connection, transaction))
                        {
                            command3.Parameters.AddWithValue("@Siirtomaksu", siirtomaksu);
                            command3.Parameters.AddWithValue("@Siirto", siirto);
                            command3.Parameters.AddWithValue("@Käyttömaksu", käyttömaksu);
                            command3.Parameters.AddWithValue("@Käyttö", käyttö);

                            await command3.ExecuteNonQueryAsync();
                        }

                        using (SqlCommand command4 = new SqlCommand(queryTable4, connection, transaction))
                        {
                            command4.Parameters.AddWithValue("@Nimi", kodinkone);
                            command4.Parameters.AddWithValue("@Max_Teho", kodinkoneenTeho);

                            await command4.ExecuteNonQueryAsync();
                        }

                        await transaction.CommitAsync();

                        MessageBox.Show("All data saved successfully to all tables!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            await transaction.RollbackAsync();
                        }
                        catch (Exception rollbackEx)
                        {
                        }

                        MessageBox.Show("Database error. Changes were rolled back: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        } // utton1_clickin loppu

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) // Tämä tapahtuma tarkistaa onko autontyyppi "Täyssähkö" ja jos on, niin on mahdollista valita akun koko,
                                                                                // muuten akun koko on pois käytöstä ja sen arvo nollataan
        {
            if (comboBox2.SelectedItem.ToString() == "Täyssähkö")
            {
                numericUpDown5.Enabled = true;
            }
            else
            {
                numericUpDown5.Enabled = false;
                numericUpDown5.Value = 0;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string laite = textBox1.Text;
            decimal maxTeho = numericUpDown10.Value;

            string connectionString =
                 "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Sähkötiedot;" +
                 "Integrated Security=True;Pooling=False;Encrypt=False;TrustServerCertificate=True;";

            string query = "INSERT INTO Laite (Nimi, Max_Teho) VALUES (@Nimi, @Max_Teho)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nimi", laite);
                    command.Parameters.AddWithValue("@Max_Teho", maxTeho);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Virhe tallennettaessa laitetta: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                textBox1.Clear();
                numericUpDown10.Value = 0;
            }
        }
    }
}