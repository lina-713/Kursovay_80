using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Kursovay_80
{
    public partial class InfStadion : Form
    {
        private readonly NpgsqlConnection connection;
        public InfStadion(NpgsqlConnection npgsqlConnection)
        {
            InitializeComponent();
            connection = npgsqlConnection;
            var n = npgsqlConnection.UserName;
            FillGrid();
            if (n == "guest")
            {
                button1.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            AddStadion stadionAdd = new AddStadion(connection, this, null);
            stadionAdd.Show();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            AddStadion updateStadion = new AddStadion(connection, this, id);
            updateStadion.Show();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            var stadionDelete = MessageBox.Show("Вы уверены, что хотите удалить этот стадион?", "Удаление стадиона", MessageBoxButtons.YesNo);
            if (stadionDelete == DialogResult.Yes)
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand("delete_stadion", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@delete_id", id);
                command.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Стадион удален!");
                FillGrid();
            }
        }
        public void FillGrid()
        {
            List<ViewStadions> listStadions = new List<ViewStadions>();
            connection.Open();
            string sql = "select * from fill_stadions('id_stadion','city','capacity','name')";
            NpgsqlCommand command = new NpgsqlCommand(sql, connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                ViewStadions viewStadions = new ViewStadions()
                {
                    Id = Convert.ToInt32(reader["id_stadion"]),
                    City = Convert.ToString(reader["citu"]),                    
                    Capacity = Convert.ToInt32(reader["capacity"]),
                    Name = Convert.ToString(reader["name"]),
                    CountMatch = Convert.ToInt32(reader["count_matches"])
                };
                listStadions.Add(viewStadions);
            }
            dataGridView1.DataSource = listStadions;
            dataGridView1.Columns[0].Visible = false;
            connection.Close();
        }
        private void InfStadion_Load(object sender, EventArgs e)
        {

        }
        private void button5_Click(object sender, EventArgs e)
        {
            FillGrid();
        }

        private void fileSystemWatcher1_Changed(object sender, System.IO.FileSystemEventArgs e)
        {

        }
    }
}
