using Npgsql;
using System;
using System.Windows.Forms;

namespace Kursovay_80
{
    public partial class MainWindow : Form
    {
        private readonly NpgsqlConnection connection;
        public MainWindow(NpgsqlConnection npgsqlConnection)
        {
            InitializeComponent();
            connection = npgsqlConnection;
        }
        private void button1_Click(object sender, EventArgs e)
        { 
            InfStadion infStadion = new InfStadion(connection);
            infStadion.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            InfAthlet infAthlet = new InfAthlet(connection);
            infAthlet.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ResultsM resultsM = new ResultsM(connection);
            resultsM.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            InfTeam infTeam = new InfTeam(connection);
            infTeam.Show();
        }
    }
}
