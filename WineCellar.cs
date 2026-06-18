using System;
using System.Collections.Generic;
using System.Linq;

namespace WineCollector
{
    public class WineCellar
    {
        private List<Wine> _wines = new List<Wine>();
        private Dictionary<string, List<Wine>> _byRegion = new Dictionary<string, List<Wine>>();
        private Dictionary<string, List<Wine>> _byProducer = new Dictionary<string, List<Wine>>();
        private SortedDictionary<int, List<Wine>> _byVintage = new SortedDictionary<int, List<Wine>>();
        private Queue<Wine> _consumptionQueue = new Queue<Wine>();
        private object _lock = new object();

        public string Name { get; set; } = "Moj vinski podrum";
        public int MaxCapacity { get; set; } = 200;

        public event EventHandler<Wine> WineReady;

        public void AddWine(Wine wine)
        {
            if (wine.Vintage < 1800 || wine.Vintage > DateTime.Now.Year)
                throw new InvalidVintageException(wine.Vintage);

            lock (_lock)
            {
                _wines.Add(wine);

                if (!_byRegion.ContainsKey(wine.Region))
                    _byRegion[wine.Region] = new List<Wine>();
                _byRegion[wine.Region].Add(wine);

                if (!_byProducer.ContainsKey(wine.Producer))
                    _byProducer[wine.Producer] = new List<Wine>();
                _byProducer[wine.Producer].Add(wine);

                if (!_byVintage.ContainsKey(wine.Vintage))
                    _byVintage[wine.Vintage] = new List<Wine>();
                _byVintage[wine.Vintage].Add(wine);
            }
        }

        public void RemoveWine(Wine wine)
        {
            lock (_lock)
            {
                _wines.Remove(wine);

                if (_byRegion.ContainsKey(wine.Region))
                    _byRegion[wine.Region].Remove(wine);

                if (_byProducer.ContainsKey(wine.Producer))
                    _byProducer[wine.Producer].Remove(wine);

                if (_byVintage.ContainsKey(wine.Vintage))
                    _byVintage[wine.Vintage].Remove(wine);
            }
        }

        public List<Wine> GetAll()
        {
            return _wines;
        }

        public List<Wine> GetByRegion(string region)
        {
            if (_byRegion.ContainsKey(region))
                return _byRegion[region];
            return new List<Wine>();
        }

        public List<Wine> GetByProducer(string producer)
        {
            if (_byProducer.ContainsKey(producer))
                return _byProducer[producer];
            return new List<Wine>();
        }

        public List<Wine> GetByVintage(int vintage)
        {
            if (_byVintage.ContainsKey(vintage))
                return _byVintage[vintage];
            return new List<Wine>();
        }

        public SortedDictionary<int, List<Wine>> GetByVintageAll()
        {
            return _byVintage;
        }

        public Dictionary<string, List<Wine>> GetByRegionAll()
        {
            return _byRegion;
        }

        public Dictionary<string, List<Wine>> GetByProducerAll()
        {
            return _byProducer;
        }

        public List<Wine> GetReadyToDrink()
        {
            var ready = new List<Wine>();
            foreach (var wine in _wines)
            {
                var status = wine.GetMaturityStatus();
                if (status == MaturityStatus.Optimalno || status == MaturityStatus.UlazakUOptimum)
                    ready.Add(wine);
            }
            return ready;
        }

        public void EnqueueForConsumption(Wine wine)
        {
            _consumptionQueue.Enqueue(wine);
        }

        public Wine DequeueNextConsumption()
        {
            if (_consumptionQueue.Count > 0)
                return _consumptionQueue.Dequeue();
            return null;
        }

        public int QueueCount => _consumptionQueue.Count;

        public void CheckMaturity()
        {
            foreach (var wine in _wines)
            {
                var status = wine.GetMaturityStatus();
                if (status == MaturityStatus.Optimalno || status == MaturityStatus.UlazakUOptimum)
                    WineReady?.Invoke(this, wine);
            }
        }

        public double GetTotalValue()
        {
            double total = 0;
            foreach (var wine in _wines)
                total += wine.GetTotalValue();
            return total;
        }

        public int TotalBottles
        {
            get
            {
                int total = 0;
                foreach (var wine in _wines)
                    total += wine.Quantity;
                return total;
            }
        }

        public double FillPercentage => (double)TotalBottles / MaxCapacity * 100;

        public int Count => _wines.Count;
    }
}