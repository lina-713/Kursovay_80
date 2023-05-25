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
        public InfStadion(NpgsqlConnection npgsqlConnection)
        {
            InitializeComponent();
            connection = npgsqlConnection;
            FillGrid();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StadionAdd stadionAdd = new StadionAdd(connection);
            stadionAdd.Show();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            FillGrid();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            int q = Convert.ToInt32(dataGridView1.SelectedRows[0].Index);
            UpdateStadion updateStadion = new UpdateStadion(connection, q + 1, this);
            updateStadion.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int q = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].RowIndex);
            var stadionDelete = MessageBox.Show("Вы уверены, что хотите удалить этот стадион?", "Удаление стадиона", MessageBoxButtons.YesNo);
            if(stadionDelete == DialogResult.Yes)
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand("delete_stadion", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@delete_id", q + 1);
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
            string sql = "select city, capacity, name, count_matches from view_stadion";
            NpgsqlCommand command = new NpgsqlCommand(sql, connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                ViewStadions viewStadions = new ViewStadions()
                {
                    City = Convert.ToString(reader["city"]),
                    Capacity = Convert.ToInt32(reader["capacity"]),
                    Name = Convert.ToString(reader["name"]),
                    CountMatch = Convert.ToInt32(reader["count_matches"])
                };
                listStadions.Add(viewStadions);
            }
            dataGridView1.DataSource = listStadions;
            connection.Close();
        }
    }
}
