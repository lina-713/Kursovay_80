using Npgsql;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace Kursovay_80
{
    public partial class AddStadion : Form
    {
        private readonly NpgsqlConnection connection;
        private readonly InfStadion stadion;
        private readonly int? Id;
        public AddStadion(NpgsqlConnection npgsqlConnection, InfStadion inf, int? id)
        {
            InitializeComponent();
            connection = npgsqlConnection;
            stadion = inf;
            Id = id;

            if (Id.HasValue)
                this.Text = "Изменение стадиона";

            if (Id != null)
            {
                string str = $"select city, capacity, name from info_about_location where id_stadion = {Id}";
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
        }
        private void button1_Click(object sender, EventArgs e)
        {   
            connection.Open();
            if (Id.HasValue)
                UpdateStadion(Id.Value);
            else
                AddNewStadion();
            
        }
        private void AddNewStadion()
        {
            NpgsqlCommand command = new NpgsqlCommand("add_new_stadion", connection);
            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@newname", textBox1.Text);
                command.Parameters.AddWithValue("@newcity", textBox2.Text);
                command.Parameters.AddWithValue("@newcapacity", Convert.ToInt32(textBox3.Text));
                command.ExecuteNonQuery();
                MessageBox.Show("Стадион добавлен!");
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                connection.Close();
            }
            this.Close();
            stadion.FillGrid();
        }
       
        private void UpdateStadion(int i)
        {
            string str = "update_stadion";
            var command = new NpgsqlCommand(str, connection);
            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@new_city", textBox1.Text);
                command.Parameters.AddWithValue("@new_capacity", Convert.ToInt32(textBox2.Text));
                command.Parameters.AddWithValue("@new_name", textBox3.Text);
                command.Parameters.AddWithValue("@new_id", i);
                command.ExecuteNonQuery();
                MessageBox.Show("Стадион изменен!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            stadion.FillGrid();
            this.Close();
        }
    }
}
