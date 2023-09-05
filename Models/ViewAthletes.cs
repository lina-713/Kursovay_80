using System.Collections.Generic;
using System.ComponentModel;
using Npgsql;

namespace Kursovay_80
{
    public class ViewAthletes
    {
        public int Id { get; set; }
        [DisplayName("Фамилия")]
        public string FirstName { get; set; }
        [DisplayName("Имя")]
        public string Name { get; set; }
        [DisplayName("Рост")]
        public int Height { get; set; }
        [DisplayName("Вес")]
        public int Weight { get; set; }
        [DisplayName( "Название команды")]
        public string NameTeam { get; set; }
        static public List<string> ComboboxValue(NpgsqlConnection connection, string str)
        {
            connection.Open();
            var teamList = new List<string>();
            NpgsqlCommand command = new NpgsqlCommand(str, connection);
            NpgsqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                teamList.Add(reader.GetString(0));
            }
            connection.Close();
            return teamList;
        }
    }

}
