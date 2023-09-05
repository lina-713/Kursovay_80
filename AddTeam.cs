using Npgsql;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace Kursovay_80
{
    public partial class AddTeam : Form
    {
        private readonly NpgsqlConnection connection;
        private readonly InfTeam team;
        private readonly int? Id;
        public AddTeam(NpgsqlConnection npgsqlConnection,InfTeam infTeam, int? id )
        {
            InitializeComponent();
            connection = npgsqlConnection;
            team = infTeam;
            Id = id;
            if (Id.HasValue)
                this.Text = "Изменение команда";
            else
                this.Text = "Добавление команды";
            if ( Id != null )
            {
                EnterTeam(Id.Value);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (Id.HasValue)
                UpdateTeam(Id.Value);
            else
                AddTeams();

            team.FillGrid();
        }
        private void AddTeams()
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
        }
        private void UpdateTeam(int id)
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
                command.Parameters.AddWithValue("@up_id", id);
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
        }
        private void EnterTeam(int Id)
        {
            string str = $"SELECT name_team, date_of_foundation, coach_lastname, coach_name from teams where idteam = all(select idteam from teams where idteam = '{Id}')";
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
    }
}
