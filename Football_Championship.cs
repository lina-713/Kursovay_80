using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace Kursovay_80
{
    public class Athletes
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Name { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
        public int IdTeam { get; set; }

    }
    public class InfoaboutLocation
    {
        public int IdStadion { get; set; }
        public string City { get; set; }
        public int Capacity { get; set; }
        public string Name { get; set; }
    }
    public class MatchSchedule
    {
        public int Id { get; set; }
        public int IdFirstTeam { get; set; }
        public int IdSecondTeam { get; set; }
        public DateTime DateOfMatch { get; set; }
        public DateTime TimeOfMatch { get; set; }
        public int FirstTeamScore { get; set; }
        public int SecondTeamScore { get; set; }

    }
    public class Teams
    {
        public int Id { get; set; }
        public string NameTeam { get; set; }
        public DateTime DateOfFoundation { get; set; }
        public string CoachLastName { get; set; }
        public string CoachName { get; set; }

    }
    public class ViewResultsMatch
    {
        [DisplayName("Команда 1")]
        public string Team1 { get; set; }
        [DisplayName("Команда 2")]
        public string Team2 { get; set; }
        [DisplayName("Город проведения")]
        public string City { get; set; }
        [DisplayName("Название стадиона")]
        public string Name { get; set; }
        [DisplayName("Дата проведения матча")]
        public string DateOfMatch { get; set; }
        [DisplayName("Время проведения матча")]
        public string TimeOfMatch { get; set; }
        [DisplayName("Счет первой команды")]
        public int Team1Score { get; set; }
        [DisplayName("Счет второй команды")]
        public int Team2Score { get; set; }
        [DisplayName("Результат встречи")]
        public string ResultScore { get; set; }
    }
    public class ViewStadions
    {
        public int Id { get; set; }
        [DisplayName("Город")]
        public string City { get; set; }
        [DisplayName("Вместимость")]
        public int Capacity { get; set; }
        [DisplayName("Название")]
        public string Name { get; set; }
        [DisplayName("Количество матчей")]
        public int CountMatch { get; set; }
    }
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
    public class TeamsDictionary
    {
        private string _ikey;
        public string IKey
        {
            get
            {
                return _ikey;
            }
            set
            {
                _ikey = value;
            }
        }
        private string _ivalue;
        public string IValue
        {
            get
            {
                return _ivalue;
            }
            set
            {
                _ivalue = value;
            }
        }
        public override string ToString()
        {
            return _ivalue;
        }
    }
    public class ComboBoxIteams
    {
        public int IdTeam { get; set; }
        public string NameTeam { get; set; }
    }

}
