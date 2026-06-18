using System;
using System.Collections.Generic;


namespace WineCollector
{
    public abstract class Wine : ITasteable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Vintage { get; set; }
        public string Producer { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string Grape { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public WineType WineType { get; set; }
        public string CellarPosition { get; set; }
        public string LabelImagePath { get; set; }
        public string Notes { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Today;
        public string[] AromaticProfile { get; set; } = new string[0];

        private List<TastingNote> _tastingNotes = new List<TastingNote>();

  
        public abstract (int Start, int End) GetDrinkingWindow();
        public abstract MaturityStatus GetMaturityStatus();

        public virtual string FormatLabel()
        {
            var window = GetDrinkingWindow();
            double rating = CalculateRating();

            string result = "";
            result += $"Naziv:      {Name}\n";
            result += $"Godišnjak:  {Vintage}\n";
            result += $"Vrsta:      {WineType}\n";
            result += $"Regija:     {Region}, {Country}\n";
            result += $"Proizvođač: {Producer}\n";
            result += $"Sorta:      {Grape}\n";
            result += $"Cijena:     {Price:F2} EUR  |  Boca: {Quantity}\n";
            result += $"Prozor:     {window.Start} – {window.End}\n";
            result += $"Status:     {GetMaturityStatus()}\n";

            if (rating > 0)
                result += $"Ocjena:     {rating:F1} / 100\n";

            return result;
        }


        public void RecordTasting(TastingNote note)
        {
            _tastingNotes.Add(note);
        }

        public List<TastingNote> GetTastingNotes()
        {
            return _tastingNotes;
        }

        public double CalculateRating()
        {
            if (_tastingNotes.Count == 0)
                return 0;

            double total = 0;
            foreach (var note in _tastingNotes)
                total += note.Score;

            return total / _tastingNotes.Count;
        }

        public string[] GetAromaticProfile()
        {
            return AromaticProfile;
        }

        public double GetTotalValue()
        {
            return Price * Quantity;
        }

        public override string ToString()
        {
            return $"{Vintage} – {Name} ({Region}) [{WineType}]";
        }
    }
}