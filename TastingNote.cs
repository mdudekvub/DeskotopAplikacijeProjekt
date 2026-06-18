using System;

namespace WineCollector
{
    public class TastingNote
    {
        public int Id { get; set; }
        public int WineId { get; set; }
        public DateTime Date { get; set; }
        public string Taster { get; set; }
        public string Visual { get; set; }
        public string Nose { get; set; }
        public string Palate { get; set; }
        public string Finish { get; set; }
        public double Score { get; set; }
        public RatingScale Scale { get; set; }

        public override string ToString()
        {
            return $"[{Date:dd.MM.yyyy}] {Taster} – {Scale}: {Score:F1}/100";
        }
    }
}