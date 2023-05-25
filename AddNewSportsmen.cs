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
    public partial class AddNewSportsmen : Form
    {
        private readonly NpgsqlConnection connection;
        public AddNewSportsmen(NpgsqlConnection npgsqlConnection)
        {
            InitializeComponent();
            connection = npgsqlConnection;
            string str = "SELECT name_team FROM teams ORDER BY idteam ASC ";
            var teamList = ViewAthletes.ComboboxValue(connection, str);
            ObservableCollection<TeamsDictionary> dictionaries = new ObservableCollection<TeamsDictionary>();
            teamList.ForEach(NameTeam => dictionaries.Add(new TeamsDictionary() { IKey = String.Empty, IValue = NameTeam }));
            comboBox1.DataSource = dictionaries.ToList();
        }
        private void button1_Click(object sender, EventArgs e)// Add
        {
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand("add_new_athlet", connection);
            try 
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@newfirstname", textBox1.Text);
                command.Parameters.AddWithValue("@newname", textBox3.Text);
                command.Parameters.AddWithValue("@newheight", Convert.ToInt32(textBox2.Text));
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
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }
        
    }
}
