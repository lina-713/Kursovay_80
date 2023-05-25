using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace Kursovay_80
{
    public partial class UpdateStadion : Form
    {
        private NpgsqlConnection connection;
        private InfStadion infStadion;
        private int updateId;
        public UpdateStadion(NpgsqlConnection npgsqlConnection, int id, InfStadion stadion)
        {
            InitializeComponent();
            connection = npgsqlConnection;
            infStadion = stadion;
            updateId = id;
            string str = $"select city, capacity, name from info_about_location where id_stadion = {updateId}";
            NpgsqlCommand command = new NpgsqlCommand(str, connection);
            try
            {
                connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();
                reader.Read();
                textBox1.Text = reader.GetString(0);
                textBox2.Text = reader.GetInt32(1).ToString();
                textBox3.Text = reader.GetString(2);
                reader.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            {
                connection.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = "update_stadion";
            NpgsqlCommand command = new NpgsqlCommand(str, connection);
            try
            {
                connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@new_city", textBox1.Text);
                command.Parameters.AddWithValue("@new_capacity", Convert.ToInt32(textBox2.Text));
                command.Parameters.AddWithValue("@new_name", textBox3.Text);
                command.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                MessageBox.Show("Стадион добавлен!");
            }
            connection.Close();
            infStadion.FillGrid();
            this.Close();
        }
    }
}
