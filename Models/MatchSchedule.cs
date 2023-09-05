using System;

namespace Kursovay_80
{
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

}
