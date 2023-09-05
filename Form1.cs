using Npgsql;
using System;
using System.Windows.Forms;
 
namespace Kursovay_80
{
    public partial class Form1 : Form
    {
        public NpgsqlConnection connection;

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
                default:
                    MessageBox.Show("Неправильный логин или пароль!");
                    return;
            }
            
            NpgsqlConnection conn = new NpgsqlConnection(str);
            connection = conn;
            this.Dispose();
            // MainWindow mainWindow = new MainWindow(connection);
            //mainWindow.Show();
        }
    }
}
