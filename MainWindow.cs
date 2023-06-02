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
using NpgsqlTypes;

namespace Kursovay_80
{
    public partial class MainWindow : Form
    {
        private readonly NpgsqlConnection connection;
        public MainWindow(NpgsqlConnection npgsqlConnection)
        {
            InitializeComponent();
            connection = npgsqlConnection;
            Form1 form1 = new Form1();
            form1.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InfStadion infStadion = new InfStadion(connection, this);
            infStadion.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            InfAthlet infTeam = new InfAthlet(connection);
            infTeam.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ResultsM resultsM = new ResultsM(connection);
            resultsM.Show();
            this.Close();
        }


        private void button5_Click(object sender, EventArgs e)
        {
            InfTeam infTeam = new InfTeam(connection);
            infTeam.Show();
            this.Close();
        }
    }
}
