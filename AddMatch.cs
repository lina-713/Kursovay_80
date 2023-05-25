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
    public partial class AddMatch : Form
    {
        private readonly NpgsqlConnection connection;
        public AddMatch(NpgsqlConnection npgsqlConnection)
        {
            InitializeComponent();
            connection = npgsqlConnection;
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
        }

        private void AddMatch_Load(object sender, EventArgs e)
        {
            /*insert into match_schedule(id_team1, id_team2, date_of_match, time_of_match, team1_score, team2_score, idstadion)
values(new_idteam1, new_idteam2, new_date, new_time, new_score1, new_score2, new_idstadion);*/
        }
        private void button1_Click(object sender, EventArgs e)
        {
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand("add_new_match", connection);
            if(comboBox1.SelectedIndex == comboBox2.SelectedIndex)
            {
                MessageBox.Show("Команда не может играть против самой себя!");
                return;
            }
            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@new_idteam1", comboBox1.SelectedIndex + 1 );
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
    }
}
