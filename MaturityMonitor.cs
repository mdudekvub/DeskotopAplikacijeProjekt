using System;
using System.Threading;

namespace WineCollector
{
    public delegate void WineReadyDelegate(Wine wine, DateTime detectedAt);

    public class MaturityMonitor
    {
        private WineCellar _cellar;
        private Thread _thread;
        private bool _running;

        public WineReadyDelegate OnWineReady;

        public MaturityMonitor(WineCellar cellar)
        {
            _cellar = cellar;
        }

        public void Start()
        {
            _running = true;
            _thread = new Thread(Monitor);
            _thread.IsBackground = true;
            _thread.Name = "MaturityMonitor";
            _thread.Start();
        }

        public void Stop()
        {
            _running = false;
        }

        private void Monitor()
        {
            while (_running)
            {
                foreach (var wine in _cellar.GetAll())
                {
                    var status = wine.GetMaturityStatus();
                    if (status == MaturityStatus.Optimalno ||
                        status == MaturityStatus.UlazakUOptimum)
                    {
                        OnWineReady?.Invoke(wine, DateTime.Now);
                    }
                }

                Thread.Sleep(5000);
            }
        }
    }
}