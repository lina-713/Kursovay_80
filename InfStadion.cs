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

namespace Kursovay_80
{
    public partial class InfStadion : Form
    {
        private readonly NpgsqlConnection connection;
        public InfStadion(NpgsqlConnection npgsqlConnection, MainWindow mainWindow)
        {
            InitializeComponent();
            connection = npgsqlConnection;
            FillGrid();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            StadionAdd stadionAdd = new StadionAdd(connection, this);
            stadionAdd.Show();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            MainWindow mainMenu = new MainWindow(connection);
            mainMenu.Show();
            this.Close();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            UpdateStadion updateStadion = new UpdateStadion(connection, id, this);
            updateStadion.Show();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            var stadionDelete = MessageBox.Show("Вы уверены, что хотите удалить этот стадион?", "Удаление стадиона", MessageBoxButtons.YesNo);
            if(stadionDelete == DialogResult.Yes)
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
            string sql = "select * from fill_stadion('city','capacity','name')";
            NpgsqlCommand command = new NpgsqlCommand(sql, connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                ViewStadions viewStadions = new ViewStadions()
                {
                    Id = Convert.ToInt32(reader["id_stadion"]),
                    City = Convert.ToString(reader["city"]),
                    Name = Convert.ToString(reader["name"]),
                    Capacity = Convert.ToInt32(reader["capacity"]),
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
    }
}
