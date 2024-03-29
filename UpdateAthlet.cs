﻿using Npgsql;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Kursovay_80
{
    public partial class UpdateAthlet : Form
    {
        private NpgsqlConnection connection;
        private int? _id;
        private InfAthlet _infTeam;
        public UpdateAthlet(NpgsqlConnection npgsqlConnection, int? id, InfAthlet infTeam)
        {
            InitializeComponent();
            connection = npgsqlConnection;
            _infTeam = infTeam;
            _id = id;
            string str = "SELECT name_team FROM teams ORDER BY idteam ASC ";
            var teamList = ViewAthletes.ComboboxValue(connection, str);
            ObservableCollection<TeamsDictionary> dictionaries = new ObservableCollection<TeamsDictionary>();
            teamList.ForEach(NameTeam => dictionaries.Add(new TeamsDictionary() { IKey = String.Empty, IValue = NameTeam }));
            comboBox1.DataSource = dictionaries.ToList();

            ViewAthletes viewAthletes = new ViewAthletes();
            str = $"SELECT firstname, name, height, weight, id_team from athletes where id = all(select id from athletes where firstname = '{_id}')";
            NpgsqlCommand command = new NpgsqlCommand(str,connection);
            try
            {
                connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();
                reader.Read();
                textBox1.Text = reader.GetString(1);
                textBox2.Text = reader.GetString(2);
                textBox3.Text = reader.GetInt32(3).ToString();
                textBox4.Text = reader.GetInt32(4).ToString();
                comboBox1.SelectedIndex = reader.GetInt32(5) - 1;
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
        private void button1_Click(object sender, EventArgs e)
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
                command.Parameters.AddWithValue("@new_id", _id);
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
            
            _infTeam.FillGrid();
            this.Close();
        }
    }
}
