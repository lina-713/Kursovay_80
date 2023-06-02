using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class InfAthlet : Form
    {
        private readonly NpgsqlConnection connection;
        private int ID;
        public InfAthlet(NpgsqlConnection npgsqlConnection)
        {
            InitializeComponent();
            connection = npgsqlConnection;
            FillGrid();
            var str = "SELECT name_team FROM teams ORDER BY idteam ASC ";
            var teamList = ViewAthletes.ComboboxValue(connection, str);
            var dictionaries = new ObservableCollection<TeamsDictionary>();
            teamList.ForEach(NameTeam => dictionaries.Add(new TeamsDictionary() { IKey = String.Empty, IValue = NameTeam }));
            comboBox1.DataSource = dictionaries.ToList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
            MainWindow mainWindow = new MainWindow(connection);
            mainWindow.Show();
        }

        private void button2_Click(object sender, EventArgs e)// изменение 
        {
            int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            UpdateAthlet updateAthlet = new UpdateAthlet(connection, id, this );
            updateAthlet.Show();
        }

        private void button5_Click(object sender, EventArgs e) //Открытие окна для добавления игрока
        {
            AddNewSportsmen addNewSportsmen = new AddNewSportsmen(connection, this);
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
            dataGridView1.Columns[5].Visible = true;
            connection.Close();
        }

        private void button3_Click(object sender, EventArgs e)// удаление
        {
            int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            var result = MessageBox.Show("Вы действительно хотите удалить данного спортсмена?", "Удаление", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand("delete_athlet", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@new_id", id);
                command.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Игрок удален!");
                FillGrid();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<ViewAthletes> listAthletes = new List<ViewAthletes>();
            connection.Open();
            string name = comboBox1.SelectedItem.ToString();
            string str = $"SELECT firstname, name, height, weight, name_team from view_athletes where name_team = all(select name_team FROM teams where name_team = '{name}') ";
            NpgsqlCommand command = new NpgsqlCommand(str, connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                ViewAthletes athletsinf = new ViewAthletes()
                {
                    FirstName = Convert.ToString(reader["firstname"]),
                    Name = Convert.ToString(reader["name"]),
                    Height = Convert.ToInt32(reader["height"]),
                    Weight = Convert.ToInt32(reader["weight"]),
                };
                listAthletes.Add(athletsinf);
            }
            dataGridView1.DataSource = listAthletes;
            dataGridView1.Columns[5].Visible = false;
            connection.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FillGrid();
        }
    }
}
