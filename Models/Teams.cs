using System.ComponentModel;

namespace Kursovay_80
{
    public class Teams
    {
        public int Id { get; set; }
        [DisplayName("Название команды")]
        public string NameTeam { get; set; }
        [DisplayName("Дата основания")]
        public string DateOfFoundation { get; set; }
        [DisplayName("Фамилия Тренера")]
        public string CoachLastName { get; set; }
        [DisplayName("Имя тренера")]
        public string CoachName { get; set; }
        [DisplayName("Претендент")]
        public string Contender { get; set; }

    }

}
