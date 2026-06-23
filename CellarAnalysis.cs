using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WineCollector
{
    public class CellarAnalysis
    {
        private WineCellar _cellar;
        private object _lock = new object();

        public CellarAnalysis(WineCellar cellar)
        {
            _cellar = cellar;
        }

        public async Task<double> GetTotalValueAsync()
        {
            return await Task.Run(() =>
            {
                double total = 0;
                lock (_lock)
                {
                    foreach (var wine in _cellar.GetAll())
                        total += wine.GetTotalValue();
                }
                return total;
            });
        }

        public async Task<List<string>> GetRecommendationsAsync()
        {
            return await Task.Run(() =>
            {
                var result = new List<string>();
                lock (_lock)
                {
                    foreach (var wine in _cellar.GetReadyToDrink())
                    {
                        var window = wine.GetDrinkingWindow();
                        result.Add($"{wine.Name} ({wine.Vintage}) – {wine.Region} – prozor: {window.Start}–{window.End}");
                    }
                }
                if (result.Count == 0)
                    result.Add("Nema vina u optimalnom prozoru.");
                return result;
            });
        }

        public async Task<Dictionary<string, double>> GetValueByRegionAsync()
        {
            return await Task.Run(() =>
            {
                var result = new Dictionary<string, double>();
                lock (_lock)
                {
                    foreach (var kvp in _cellar.GetByRegionAll())
                    {
                        double total = 0;
                        foreach (var wine in kvp.Value)
                            total += wine.GetTotalValue();
                        result[kvp.Key] = total;
                    }
                }
                return result;
            });
        }
    }
}