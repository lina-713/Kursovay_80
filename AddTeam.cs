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
    public partial class AddTeam : Form
    {
        private readonly NpgsqlConnection connection;
        private readonly InfTeam team;
        public AddTeam(NpgsqlConnection npgsqlConnection,InfTeam infTeam )
        {
            InitializeComponent();
            connection = npgsqlConnection;
            team = infTeam;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand("add_team", connection);
            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@new_name_team", textBox1.Text);
                command.Parameters.AddWithValue("@new_date", dateTimePicker1.Value);
                command.Parameters.AddWithValue("@new_lastname", textBox2.Text);
                command.Parameters.AddWithValue("@new_name", textBox3.Text);
                command.ExecuteNonQuery();
                MessageBox.Show("Команда добавлена!");
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
            team.FillGrid();
        }
    }
}
