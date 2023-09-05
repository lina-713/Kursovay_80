using System.ComponentModel;

namespace Kursovay_80
{
    public class ViewResultsMatch
    {
        public int Id { get; set; }
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

}
