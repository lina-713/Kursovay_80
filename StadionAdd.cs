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
        private readonly InfStadion stadion; 
        public StadionAdd(NpgsqlConnection npgsqlConnection, InfStadion inf )
        {
            InitializeComponent();
            connection = npgsqlConnection;
            stadion = inf;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            connection.Open();
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

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
            MainWindow mainWindow = new MainWindow(connection);
            mainWindow.Show();
        }
    }
}
