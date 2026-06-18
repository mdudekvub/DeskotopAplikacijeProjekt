using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WineCollector
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private async void Form1_Load(object sender, EventArgs e)
        {
            var repo = new WineRepository();

            // spremi testno vino
            var barolo = new RedWine
            {
                Name = "Barolo",
                Vintage = 2016,
                Producer = "Giacomo Conterno",
                Region = "Piemonte",
                Country = "Italija",
                Grape = "Nebbiolo",
                Price = 150.00,
                Quantity = 6,
                TaninLevel = 9,
                AcidityLevel = 8,
                OakAged = true,
                OakMonths = 36
            };

            await repo.SaveRedWineAsync(barolo);

            // učitaj iz baze
            var wines = await repo.GetAllRedWinesAsync();
            MessageBox.Show($"Boca u bazi: {wines.Count}\nPrvo vino: {wines[0].Name}");
        }
    }
    
}
