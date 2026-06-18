using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WineCollector
{
    public class WineRepository
    {
        private WineDB db;

        public WineRepository()
        {
            db = new WineDB();
        }

        public async Task<List<RedWine>> GetAllRedWinesAsync()
        {
            return await Task.Run(() => db.RedWines.ToList());
        }

        public async Task<List<WhiteWine>> GetAllWhiteWinesAsync()
        {
            return await Task.Run(() => db.WhiteWines.ToList());
        }

        public async Task<List<RoseWine>> GetAllRoseWinesAsync()
        {
            return await Task.Run(() => db.RoseWines.ToList());
        }

        public async Task<List<SparklingWine>> GetAllSparklingWinesAsync()
        {
            return await Task.Run(() => db.SparklingWines.ToList());
        }

        public async Task<List<DessertWine>> GetAllDessertWinesAsync()
        {
            return await Task.Run(() => db.DessertWines.ToList());
        }

        public async Task<List<FortifiedWine>> GetAllFortifiedWinesAsync()
        {
            return await Task.Run(() => db.FortifiedWines.ToList());
        }

        public async Task SaveRedWineAsync(RedWine wine)
        {
            db.RedWines.Add(wine);
            await db.SaveChangesAsync();
        }

        public async Task SaveWhiteWineAsync(WhiteWine wine)
        {
            db.WhiteWines.Add(wine);
            await db.SaveChangesAsync();
        }

        public async Task SaveRoseWineAsync(RoseWine wine)
        {
            db.RoseWines.Add(wine);
            await db.SaveChangesAsync();
        }

        public async Task SaveSparklingWineAsync(SparklingWine wine)
        {
            db.SparklingWines.Add(wine);
            await db.SaveChangesAsync();
        }

        public async Task SaveDessertWineAsync(DessertWine wine)
        {
            db.DessertWines.Add(wine);
            await db.SaveChangesAsync();
        }

        public async Task SaveFortifiedWineAsync(FortifiedWine wine)
        {
            db.FortifiedWines.Add(wine);
            await db.SaveChangesAsync();
        }

        public async Task SaveTastingNoteAsync(TastingNote note)
        {
            db.TastingNotes.Add(note);
            await db.SaveChangesAsync();
        }

        public async Task<List<TastingNote>> GetTastingNotesForWineAsync(int wineId)
        {
            return await Task.Run(() =>
                db.TastingNotes.Where(n => n.WineId == wineId).ToList());
        }

        public async Task DeleteRedWineAsync(RedWine wine)
        {
            db.RedWines.Remove(wine);
            await db.SaveChangesAsync();
        }

        public async Task UpdateRedWineAsync(RedWine wine)
        {
            var existing = db.RedWines.Find(wine.Id);
            if (existing == null) return;

            existing.Name = wine.Name;
            existing.Vintage = wine.Vintage;
            existing.Producer = wine.Producer;
            existing.Region = wine.Region;
            existing.Country = wine.Country;
            existing.Grape = wine.Grape;
            existing.Price = wine.Price;
            existing.Quantity = wine.Quantity;
            existing.TaninLevel = wine.TaninLevel;
            existing.AcidityLevel = wine.AcidityLevel;
            existing.OakAged = wine.OakAged;
            existing.OakMonths = wine.OakMonths;

            await db.SaveChangesAsync();
        }

        public async Task UpdateWhiteWineAsync(WhiteWine wine)
        {
            var existing = db.WhiteWines.Find(wine.Id);
            if (existing == null) return;

            existing.Name = wine.Name;
            existing.Vintage = wine.Vintage;
            existing.Producer = wine.Producer;
            existing.Region = wine.Region;
            existing.Country = wine.Country;
            existing.Grape = wine.Grape;
            existing.Price = wine.Price;
            existing.Quantity = wine.Quantity;
            existing.AcidityLevel = wine.AcidityLevel;
            existing.ResidualSugar = wine.ResidualSugar;
            existing.MalolacticFermentation = wine.MalolacticFermentation;

            await db.SaveChangesAsync();
        }

        public async Task UpdateRoseWineAsync(RoseWine wine)
        {
            var existing = db.RoseWines.Find(wine.Id);
            if (existing == null) return;

            existing.Name = wine.Name;
            existing.Vintage = wine.Vintage;
            existing.Producer = wine.Producer;
            existing.Region = wine.Region;
            existing.Country = wine.Country;
            existing.Grape = wine.Grape;
            existing.Price = wine.Price;
            existing.Quantity = wine.Quantity;

            await db.SaveChangesAsync();
        }

        public async Task UpdateSparklingWineAsync(SparklingWine wine)
        {
            var existing = db.SparklingWines.Find(wine.Id);
            if (existing == null) return;

            existing.Name = wine.Name;
            existing.Vintage = wine.Vintage;
            existing.Producer = wine.Producer;
            existing.Region = wine.Region;
            existing.Country = wine.Country;
            existing.Grape = wine.Grape;
            existing.Price = wine.Price;
            existing.Quantity = wine.Quantity;
            existing.Method = wine.Method;
            existing.Dosage = wine.Dosage;
            existing.LeeAgingMonths = wine.LeeAgingMonths;

            await db.SaveChangesAsync();
        }

        public async Task UpdateDessertWineAsync(DessertWine wine)
        {
            var existing = db.DessertWines.Find(wine.Id);
            if (existing == null) return;

            existing.Name = wine.Name;
            existing.Vintage = wine.Vintage;
            existing.Producer = wine.Producer;
            existing.Region = wine.Region;
            existing.Country = wine.Country;
            existing.Grape = wine.Grape;
            existing.Price = wine.Price;
            existing.Quantity = wine.Quantity;
            existing.ResidualSugar = wine.ResidualSugar;
            existing.Botrytis = wine.Botrytis;

            await db.SaveChangesAsync();
        }

        public async Task UpdateFortifiedWineAsync(FortifiedWine wine)
        {
            var existing = db.FortifiedWines.Find(wine.Id);
            if (existing == null) return;

            existing.Name = wine.Name;
            existing.Vintage = wine.Vintage;
            existing.Producer = wine.Producer;
            existing.Region = wine.Region;
            existing.Country = wine.Country;
            existing.Grape = wine.Grape;
            existing.Price = wine.Price;
            existing.Quantity = wine.Quantity;
            existing.Style = wine.Style;
            existing.AlcoholPercent = wine.AlcoholPercent;

            await db.SaveChangesAsync();
        }
    }
}