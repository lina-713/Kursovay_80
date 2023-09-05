using System.ComponentModel;

namespace Kursovay_80
{
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

}
