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
    public partial class InfTeam : Form
    {
        private readonly NpgsqlConnection connection;
        public InfTeam(NpgsqlConnection npgsqlConnection)
        {
            InitializeComponent();
            connection = npgsqlConnection;
            FillGrid();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
            MainWindow mainWindow = new MainWindow(connection);
            mainWindow.Show();
        }

        private void button2_Click(object sender, EventArgs e)// изменение 
        {
            int q = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            UpdateAthlet updateAthlet = new UpdateAthlet(connection, q, this );
            updateAthlet.Show();
        }

        private void button5_Click(object sender, EventArgs e) //Открытие окна для добавления игрока
        {
            AddNewSportsmen addNewSportsmen = new AddNewSportsmen(connection);
            addNewSportsmen.Show();

        }
        public void FillGrid()// вывод таблицы в datagrid
        {
            List<ViewAthletes> listAthletes = new List<ViewAthletes>();
            List<Teams> teamsList = new List<Teams>();
            connection.Open();
            string sql = "SELECT * FROM view_athletes";
            NpgsqlCommand command = new NpgsqlCommand(sql, connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                ViewAthletes athletsinf = new ViewAthletes()
                {
                    Id = Convert.ToInt32(reader["ID"]),
                    FirstName = Convert.ToString(reader["firstname"]),
                    Name = Convert.ToString(reader["name"]),
                    Height = Convert.ToInt32(reader["height"]),
                    Weight = Convert.ToInt32(reader["weight"]),
                    NameTeam = Convert.ToString(reader["name_team"])
                };
                listAthletes.Add(athletsinf);
            }
            dataGridView1.DataSource = listAthletes;
            dataGridView1.Columns[0].Visible = false;
            connection.Close();
        }

        private void button3_Click(object sender, EventArgs e)// удаление
        {
            int q = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

            var result = MessageBox.Show("Ты долбаеб?", "Ты реально долбаеб", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand("delete_athlet", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@new_id", q);
                command.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Игрок удален!");
                FillGrid();
            }
        }
    }
}
