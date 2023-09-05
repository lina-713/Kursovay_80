using Npgsql;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace Kursovay_80
{
    public partial class AddNewSportsmen : Form
    {
        private readonly NpgsqlConnection connection;
        private readonly InfAthlet athlet;
        private readonly int? Id;
        public AddNewSportsmen(NpgsqlConnection npgsqlConnection, InfAthlet inf, int? id)
        {
            InitializeComponent();
            connection = npgsqlConnection;
            athlet = inf;
            Id = id;
            TeamDictionary();
            if (Id.HasValue)
            {
                this.Text = "Обновление спортсмена";
                EnterAthletes(Id.Value);
            }
        }
        private void EnterAthletes(int id)
        {
            ViewAthletes viewAthletes = new ViewAthletes();
            string str = $"SELECT firstname, name, height, weight, id_team from athletes where id = all(select id from athletes where id = '{id}')";
            NpgsqlCommand command = new NpgsqlCommand(str, connection);
            try
            {
                connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();
                reader.Read();
                textBox1.Text = reader.GetString(0);
                textBox2.Text = reader.GetString(1);
                textBox3.Text = reader.GetInt32(2).ToString();
                textBox4.Text = reader.GetInt32(3).ToString();
                comboBox1.SelectedIndex = reader.GetInt32(4) - 1;
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
        private void TeamDictionary()
        {
            string str = "SELECT name_team FROM teams ORDER BY idteam ASC ";
            var teamList = ViewAthletes.ComboboxValue(connection, str);
            ObservableCollection<TeamsDictionary> dictionaries = new ObservableCollection<TeamsDictionary>();
            teamList.ForEach(NameTeam => dictionaries.Add(new TeamsDictionary() { IKey = String.Empty, IValue = NameTeam }));
            comboBox1.DataSource = dictionaries.ToList();
        }
        private void button1_Click(object sender, EventArgs e)// Add
        {
            
            if (Id.HasValue)
            {
                UpdateAthletes(Id.Value);
                athlet.FillGrid();
            }
            else 
            { 
                AddAthletes();
                athlet.FillGrid();
            }
        }
        public void UpdateAthletes(int id)
        {
            string str = "update_athlet";
            NpgsqlCommand command = new NpgsqlCommand(str, connection);
            try
            {
                connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@new_firstname", textBox1.Text);
                command.Parameters.AddWithValue("@new_name", textBox2.Text);
                command.Parameters.AddWithValue("@new_height", Convert.ToInt32(textBox3.Text));
                command.Parameters.AddWithValue("@new_weight", Convert.ToInt32(textBox4.Text));
                command.Parameters.AddWithValue("@new_idteam", comboBox1.SelectedIndex + 1);
                command.Parameters.AddWithValue("@new_id", id);
                command.ExecuteNonQuery();
                MessageBox.Show("Игрок изменен!");
            }
            catch (Exception exp)
            {

                MessageBox.Show(exp.Message);
            }
            finally
            {
                connection.Close();
            }
            athlet.FillGrid();
            this.Close();
        }
       

        public void AddAthletes()
        {
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand("add_new_athlet", connection);
            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@newfirstname", textBox1.Text);
                command.Parameters.AddWithValue("@newname", textBox2.Text);
                command.Parameters.AddWithValue("@newheight", Convert.ToInt32(textBox3.Text));
                command.Parameters.AddWithValue("@newweight", Convert.ToInt32(textBox4.Text));
                command.Parameters.AddWithValue("@newid_team", comboBox1.SelectedIndex + 1);
                command.ExecuteNonQuery();
                MessageBox.Show("Игрок добавлен!");
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
            finally
            {
                connection.Close();
            }
            athlet.FillGrid();
            this.Close();
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char letter = e.KeyChar;

            if (Char.IsLetter(letter))
            {
                e.Handled = true;
            }
        }
    }
}
