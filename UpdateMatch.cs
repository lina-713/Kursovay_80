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

namespace Kursovay_80
{
    public partial class UpdateMatch : Form
    {
        private readonly NpgsqlConnection connection;
        private int id;
        private ResultsM resultsM;
        public UpdateMatch(NpgsqlConnection npgsqlConnection, int idRow, ResultsM results)
        {
            InitializeComponent();
            connection = npgsqlConnection;
            id = idRow;
            resultsM = results;
            string str = "SELECT name_team FROM teams ORDER BY idteam ASC ";
            string sql = "SELECT name FROM info_about_location ORDER BY id_stadion ASC";
            var teamList = ViewAthletes.ComboboxValue(connection, str);
            var stadionList = ViewAthletes.ComboboxValue(connection, sql);
            ObservableCollection<TeamsDictionary> dictionaries = new ObservableCollection<TeamsDictionary>();
            teamList.ForEach(NameTeam => dictionaries.Add(new TeamsDictionary() { IKey = String.Empty, IValue = NameTeam }));
            comboBox1.DataSource = dictionaries.ToList();
            comboBox2.DataSource = dictionaries.ToList();
            dictionaries = new ObservableCollection<TeamsDictionary>();
            stadionList.ForEach(Name => dictionaries.Add(new TeamsDictionary() { IKey = String.Empty, IValue = Name }));
            comboBox3.DataSource = dictionaries.ToList();

            ViewResultsMatch viewResultsMatch = new ViewResultsMatch();
            str = $"SELECT id_team1, id_team2, date_of_match, team1_score, team2_score, idstadion from match_schedule where id = {id} ";
            NpgsqlCommand command = new NpgsqlCommand(str, connection);
            try
            {
                connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();
                reader.Read();
                comboBox1.SelectedIndex = reader.GetInt32(0) - 1;
                comboBox2.SelectedIndex = reader.GetInt32(1) - 1;
                var dy = reader.GetDateTime(2);
                var t = dy.TimeOfDay;
                dateTimePicker1.Value = dy.Date;
                dateTimePicker2.CustomFormat = "HH:mm";
                dateTimePicker2.Value = Convert.ToDateTime(t.ToString());
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
        public void da()
        {

        }

    }
}
