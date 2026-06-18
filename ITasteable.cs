using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WineCollector
{
    internal interface ITasteable
    {
        void RecordTasting(TastingNote note);
        List<TastingNote> GetTastingNotes();
        double CalculateRating();
        string[] GetAromaticProfile();
    }
}
