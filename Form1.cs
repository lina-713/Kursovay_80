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
    public partial class Form1 : Form
    {
        internal NpgsqlConnection connection;

        public Form1()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            string str;
            string log = textBox1.Text;
            string pass = textBox2.Text;
            
            switch (log + pass)
            {
                case ("guest" + "guest"):
                    str = "Host=localhost;Port=5432;Database=The_information_system_of_the_Russian_Football_Championship;Username=guest;Password=guest";
                    break;

                case ("admin" + "admin"):
                    str = "Host=localhost;Port=5432;Database=The_information_system_of_the_Russian_Football_Championship;Username=admin;Password=admin";
                    break;

                case ("employee" + "employee"):
                    str = "Host=localhost;Port=5432;Database=The_information_system_of_the_Russian_Football_Championship;Username=employee;Password=employee";
                    break;
                default:
                    str = null;
                    break;
            }

            if (str == null)
            {
                MessageBox.Show("Error!");
                return;
            }
            NpgsqlConnection connection = new NpgsqlConnection(str);
            MainWindow mainWindow = new MainWindow(connection);
            mainWindow.Show();
            
                
        }
    }
}
