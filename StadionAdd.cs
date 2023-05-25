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
using NpgsqlTypes;


namespace Kursovay_80
{
    public partial class StadionAdd : Form
    {
        private readonly NpgsqlConnection connection;
        public StadionAdd(NpgsqlConnection npgsqlConnection)
        {
            InitializeComponent();
            connection = npgsqlConnection;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand("add_stadion", connection);
            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@namestadion", textBox1.Text);
                command.Parameters.AddWithValue("@citystadion", textBox2.Text);
                command.Parameters.AddWithValue("@capacitystadion", Convert.ToInt32(textBox3.Text));
                command.ExecuteNonQuery();
            }
            catch (Exception exc)
            {

                MessageBox.Show(exc.Message);
            }
            finally
            {
                MessageBox.Show("Стадион добавлен!");
            }
            connection.Close();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
            MainWindow mainWindow = new MainWindow(connection);
            mainWindow.Show();
        }
    }
}
