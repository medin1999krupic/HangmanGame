namespace HangMan
{
    public class Score
    {

        public Score()
        {
            Matches = 0;
            Wins = 0;
            Losts = 0;
            Points = 0;
            KD = 0;
        }
        public int Matches { get; set; }
        public int Wins { get; set; }

        public int Losts { get; set; }

        public int Points { get; set; }

        public int KD { get; set; }
    }
}