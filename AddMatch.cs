using Npgsql;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace Kursovay_80
{
    public partial class AddMatch : Form
    {
        private readonly NpgsqlConnection connection;
        private readonly ResultsM resultsM;
        private readonly int? Id;
        public AddMatch(NpgsqlConnection npgsqlConnection, ResultsM results, int? id)
        {
            InitializeComponent();
            connection = npgsqlConnection;
            resultsM = results;
            Id = id;
            TeamsDictionary();
            StadionDictionary();
            if (Id.HasValue)
                this.Text = "Изменение матча";

            if (Id != null)
            {
                EnterMatch(Id.Value);
            }
        }
        private void EnterMatch(int? Id)
        {
            ViewResultsMatch viewResultsMatch = new ViewResultsMatch();
            string str = $"SELECT id_team1, id_team2, date_of_match, team1_score, team2_score, idstadion from match_schedule where id = {Id} ";
            NpgsqlCommand command = new NpgsqlCommand(str, connection);
            try
            {
                connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();
                reader.Read();
                comboBox1.SelectedIndex = reader.GetInt32(0) - 1;
                comboBox2.SelectedIndex = reader.GetInt32(1) - 1;
                var dy = reader.GetDateTime(2);
                var time = dy.TimeOfDay;
                dateTimePicker1.Value = dy.Date;
                dateTimePicker2.Value = DateTime.Parse(time.ToString("t"));
                textBox1.Text = reader.GetInt32(3).ToString();
                textBox2.Text = reader.GetInt32(4).ToString();
                comboBox3.SelectedIndex = reader.GetInt32(5) - 1;

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
            connection.Close();
        }
        private void TeamsDictionary()
        {
            string str = "SELECT name_team FROM teams ORDER BY idteam ASC ";
            var teamList = ViewAthletes.ComboboxValue(connection, str);
            ObservableCollection<TeamsDictionary> dictionaries = new ObservableCollection<TeamsDictionary>();

            teamList.ForEach(NameTeam => dictionaries.Add(new TeamsDictionary() { IKey = String.Empty, IValue = NameTeam }));
            comboBox1.DataSource = dictionaries.ToList();
            comboBox2.DataSource = dictionaries.ToList();
        }
        private void StadionDictionary()
        {
            string str = "SELECT name FROM info_about_location ORDER BY id_stadion ASC";
            var stadionList = ViewAthletes.ComboboxValue(connection, str);
            ObservableCollection<TeamsDictionary> dictionaries = new ObservableCollection<TeamsDictionary>();
            stadionList.ForEach(Name => dictionaries.Add(new TeamsDictionary() { IKey = String.Empty, IValue = Name }));
            comboBox3.DataSource = dictionaries.ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Id.HasValue)
                UpdateMatch(Id.Value);
            else
                AddMatches();
        }
        private void UpdateMatch(int id)
        {
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand("update_match", connection);
            if (comboBox1.SelectedIndex == comboBox2.SelectedIndex)
            {
                MessageBox.Show("Команда не может играть против самой себя!");
                return;
            }
            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@new_idteam1", comboBox1.SelectedIndex + 1);
                command.Parameters.AddWithValue("@new_idteam2", comboBox2.SelectedIndex + 1);
                var d = dateTimePicker1.Value.Date;
                var t = dateTimePicker2.Value.TimeOfDay;
                command.Parameters.AddWithValue("@new_date", d + t);
                command.Parameters.AddWithValue("@new_score1", Convert.ToInt32(textBox1.Text));
                command.Parameters.AddWithValue("@new_score2", Convert.ToInt32(textBox2.Text));
                command.Parameters.AddWithValue("@new_idstadion", comboBox3.SelectedIndex + 1);
                command.Parameters.AddWithValue("@_id", id);
                command.ExecuteNonQuery();
                MessageBox.Show("Матч обновлен!");
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
            finally
            {
                connection.Close();
            }
            resultsM.FillGrid();
            this.Close();
        }
        private void AddMatches()
        {
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand("add_new_match", connection);
            if (comboBox1.SelectedIndex == comboBox2.SelectedIndex)
            {
                MessageBox.Show("Команда не может играть против самой себя!");
                return;
            }
            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@new_idteam1", comboBox1.SelectedIndex + 1);
                command.Parameters.AddWithValue("@new_idteam2", comboBox2.SelectedIndex + 1);
                var d = dateTimePicker1.Value.Date;
                var t = dateTimePicker2.Value.TimeOfDay;
                command.Parameters.AddWithValue("@new_date", d + t);
                command.Parameters.AddWithValue("@new_score1", Convert.ToInt32(textBox1.Text));
                command.Parameters.AddWithValue("@new_score2", Convert.ToInt32(textBox2.Text));
                command.Parameters.AddWithValue("@new_idstadion", comboBox3.SelectedIndex + 1);
                command.ExecuteNonQuery();
                MessageBox.Show("Матч добавлен!");
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
            finally
            {
                connection.Close();
            }
            resultsM.FillGrid();
            this.Close();
        }
       
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }
        private void AddMatch_Load(object sender, EventArgs e)
        {
        }
    } 
}
