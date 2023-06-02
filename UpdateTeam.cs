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
    public partial class UpdateTeam : Form
    {
        private readonly NpgsqlConnection connection;
        private readonly int _id;
        private readonly InfTeam infTeam;
        public UpdateTeam(NpgsqlConnection npgsqlConnection, InfTeam team, int id)
        {
            InitializeComponent();
            connection = npgsqlConnection;
            _id = id;
            infTeam = team;
            string str = $"SELECT name_team, date_of_foundation, coach_lastname, coach_name from teams where idteam = all(select idteam from teams where idteam = '{_id}')";
            NpgsqlCommand command = new NpgsqlCommand(str, connection);
            try
            {
                connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();
                reader.Read();
                textBox1.Text = reader.GetString(0);
                dateTimePicker1.Value = reader.GetDateTime(1);
                textBox2.Text = reader.GetString(2);
                textBox3.Text = reader.GetString(3);
                reader.Close();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand("update_teams", connection);
            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@up_name_team", textBox1.Text);
                command.Parameters.AddWithValue("@up_date", dateTimePicker1.Value);
                command.Parameters.AddWithValue("@up_coach_lastname", textBox2.Text);
                command.Parameters.AddWithValue("@up_coach_name", textBox3.Text);
                command.Parameters.AddWithValue("@up_id", _id);
                command.ExecuteNonQuery();
                MessageBox.Show("Команда обновлена!");
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
            infTeam.FillGrid();
        }
    }
}
