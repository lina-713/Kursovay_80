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
    public partial class ResultsM : Form
    { 
        private readonly NpgsqlConnection connection;
        public ResultsM(NpgsqlConnection npgsqlConnection)
        {
            InitializeComponent();
            connection = npgsqlConnection;
            FillGrid();
        }

        private void ResultsM_Load(object sender, EventArgs e)
        {
        }
        public void FillGrid()
        {
            List<ViewResultsMatch> matches = new List<ViewResultsMatch>();
            connection.Open();
            string sql = "SELECT * FROM match_results ";
            NpgsqlCommand command = new NpgsqlCommand(sql, connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                ViewResultsMatch viewR = new ViewResultsMatch()
                {
                    Team1 = Convert.ToString(reader["team1"]),
                    Team2 = Convert.ToString(reader["team2"]),
                    City = Convert.ToString(reader["city"]),
                    Name = Convert.ToString(reader["name"]),
                    DateOfMatch = Convert.ToDateTime(reader["date_of_match"]).ToString("dd/MM/yyyy"),
                    TimeOfMatch = Convert.ToDateTime(reader["date_of_match"]).ToString("HH:mm"),
                    Team1Score = Convert.ToInt32(reader["team1_score"]),
                    Team2Score = Convert.ToInt32(reader["team2_score"]),
                    ResultScore = Convert.ToString(reader["resultscore"])
                };
                matches.Add(viewR);
            }
            dataGridView1.DataSource = matches;
            connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddMatch addMatch = new AddMatch(connection);
            addMatch.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int q = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].RowIndex);
            UpdateMatch updateMatch = new UpdateMatch(connection, q, this);
            updateMatch.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int q = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].RowIndex);

            var match = MessageBox.Show("Удаление", "Вы уверены, что хотите удалить этот матч?", MessageBoxButtons.YesNo);
            if(match == DialogResult.Yes)
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand("delete_match", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@delete_id", q);
                command.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Матч удален!");
                FillGrid();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FillGrid();
        }
    }
}
