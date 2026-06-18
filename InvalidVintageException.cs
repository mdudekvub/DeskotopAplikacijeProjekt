using System;

namespace WineCollector
{
    public class InvalidVintageException : Exception
    {
        public int Vintage { get; }

        public InvalidVintageException(int vintage)
            : base($"Godišnjak {vintage} nije ispravan. Mora biti između 1800 i {DateTime.Now.Year}.")
        {
            Vintage = vintage;
        }
    }
}