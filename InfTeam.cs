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
    public partial class InfTeam : Form
    {
        private readonly NpgsqlConnection connection;
        public InfTeam(NpgsqlConnection npgsqlConnection)
        {
            InitializeComponent();
            connection = npgsqlConnection;
            FillGrid();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddTeam addTeam = new AddTeam(connection, this);
            addTeam.Show();
        }

        public void FillGrid()
        {
            List<ViewTeams> listTeams = new List<ViewTeams>();
            connection.Open();
            string sql = "SELECT * FROM view_teams";
            NpgsqlCommand command = new NpgsqlCommand(sql, connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                ViewTeams teams = new ViewTeams()
                {
                    Id = Convert.ToInt32(reader["idteam"]),
                    NameTeam = Convert.ToString(reader["name_team"]),
                    DateOfFoundation = Convert.ToDateTime(reader["date_of_foundation"]).ToString("dd/MM/yyyy"),
                    CoachLastName = Convert.ToString(reader["coach_lastname"]),
                    CoachName = Convert.ToString(reader["coach_name"]),
                    MatchesPlayed = Convert.ToInt32(reader["matches_played"]),
                    ResultScore = Convert.ToInt32(reader["results_score"]),
                    Contender = Convert.ToString(reader["contender"])
                };
                listTeams.Add(teams);
            }
            dataGridView1.DataSource = listTeams;
            dataGridView1.Columns[0].Visible = false;
            connection.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            UpdateTeam updateTeam = new UpdateTeam(connection, this, id);
            updateTeam.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            var teamsDelete = MessageBox.Show("Вы уверены, что хотите удалить эту команду?", "Удаление команды", MessageBoxButtons.YesNo);
            if (teamsDelete == DialogResult.Yes)
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand("delete_teams", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@delete_id", id);
                command.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Команда удален!");
                FillGrid();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand("curs_proc", connection);//вызов курсора
            command.CommandType = CommandType.StoredProcedure;
            command.ExecuteNonQuery();
            connection.Close();
            FillGrid();
        }
    }
}
