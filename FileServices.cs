using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace WineCollector
{
    public class XmlService
    {
        public void ExportToXml(List<RedWine> wines, string path)
        {
            var serializer = new XmlSerializer(typeof(List<RedWine>), new XmlRootAttribute("Wines"));
            using (var sw = new StringWriter())
            {
                serializer.Serialize(sw, wines);
                File.WriteAllText(path, XDocument.Parse(sw.ToString()).ToString(), Encoding.UTF8);
            }
        }

        public List<RedWine> ImportFromXml(string path)
        {
            var serializer = new XmlSerializer(typeof(List<RedWine>), new XmlRootAttribute("Wines"));
            using (var sr = new StringReader(File.ReadAllText(path)))
            {
                return (List<RedWine>)serializer.Deserialize(sr);
            }
        }
    }

    public class UserSettings
    {
        public string CellarName { get; set; } = "Moj podrum";
        public int MaxCapacity { get; set; } = 200;
        public string DefaultTaster { get; set; } = "";
        public int TcpPort { get; set; } = 7777;
    }

    public class JsonService
    {
        private string _path;

        public JsonService(string path = "settings.json")
        {
            _path = path;
        }

        public void Save(UserSettings settings)
        {
            var serializer = new JavaScriptSerializer();
            File.WriteAllText(_path, serializer.Serialize(settings), Encoding.UTF8);
        }

        public UserSettings Load()
        {
            if (!File.Exists(_path))
                return new UserSettings();

            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<UserSettings>(File.ReadAllText(_path));
        }
    }

    public class CsvService
    {
        public List<RedWine> ImportFromCsv(string path)
        {
            var wines = new List<RedWine>();
            string[] lines = File.ReadAllLines(path, Encoding.UTF8);

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrEmpty(line)) continue;

                string[] cols = line.Split(',');
                if (cols.Length < 7) continue;

                var wine = new RedWine();
                wine.Name = cols[0].Trim();
                wine.Vintage = int.Parse(cols[1].Trim());
                wine.Producer = cols[2].Trim();
                wine.Region = cols[3].Trim();
                wine.Country = cols[4].Trim();
                wine.Grape = cols[5].Trim();
                wine.Price = double.Parse(cols[6].Trim());
                if (cols.Length > 7)
                    wine.Quantity = int.Parse(cols[7].Trim());

                wines.Add(wine);
            }

            return wines;
        }

        public void ExportToCsv(List<Wine> wines, string path)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Name,Vintage,Producer,Region,Country,Grape,Price,Quantity");

            foreach (var wine in wines)
            {
                sb.AppendLine($"{wine.Name},{wine.Vintage},{wine.Producer}," +
                              $"{wine.Region},{wine.Country},{wine.Grape}," +
                              $"{wine.Price.ToString(System.Globalization.CultureInfo.InvariantCulture)}," +
                              $"{wine.Quantity}");
            }

            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }
    }

    public class HtmlService
    {
        public void GenerateTastingReport(Wine wine, string path)
        {
            var window = wine.GetDrinkingWindow();
            double rating = wine.CalculateRating();
            var notes = wine.GetTastingNotes();

            var sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang='hr'><head><meta charset='UTF-8'><title>Degustacijska bilješka</title>");
            sb.AppendLine("<style>body{font-family:Georgia,serif;background:#1a1a1a;color:#e8e0d0;padding:40px}");
            sb.AppendLine("h1{color:#D4AF37}h2{color:#D4AF37;margin-top:30px}");
            sb.AppendLine(".card{background:#2a2218;border:1px solid #4a3820;border-radius:8px;padding:16px;margin:10px 0}");
            sb.AppendLine(".label{color:#8B6914;font-size:0.8rem;text-transform:uppercase}</style></head><body>");

            sb.AppendLine($"<h1>{wine.Name}</h1>");
            sb.AppendLine($"<p>{wine.WineType} · {wine.Region}, {wine.Country} · {wine.Producer}</p>");

            sb.AppendLine("<div class='card'>");
            sb.AppendLine($"<p><span class='label'>Godišnjak:</span> {wine.Vintage}</p>");
            sb.AppendLine($"<p><span class='label'>Sorta:</span> {wine.Grape}</p>");
            sb.AppendLine($"<p><span class='label'>Cijena:</span> {wine.Price:F2} EUR</p>");
            sb.AppendLine($"<p><span class='label'>Boca:</span> {wine.Quantity}</p>");
            sb.AppendLine($"<p><span class='label'>Prozor:</span> {window.Start} – {window.End}</p>");
            sb.AppendLine($"<p><span class='label'>Status:</span> {wine.GetMaturityStatus()}</p>");
            if (rating > 0)
                sb.AppendLine($"<p><span class='label'>Prosj. ocjena:</span> {rating:F1} / 100</p>");
            sb.AppendLine("</div>");

            if (wine.AromaticProfile.Length > 0)
            {
                sb.AppendLine("<h2>Aromatski profil</h2><div class='card'>");
                sb.AppendLine(string.Join(", ", wine.AromaticProfile));
                sb.AppendLine("</div>");
            }

            if (notes.Count > 0)
            {
                sb.AppendLine("<h2>Degustacijske bilješke</h2>");
                foreach (var note in notes)
                {
                    sb.AppendLine("<div class='card'>");
                    sb.AppendLine($"<p><span class='label'>Datum:</span> {note.Date:dd.MM.yyyy} · {note.Taster}</p>");
                    sb.AppendLine($"<p><span class='label'>Ocjena:</span> {note.Score:F1} {note.Scale}</p>");
                    sb.AppendLine($"<p><span class='label'>Vizual:</span> {note.Visual}</p>");
                    sb.AppendLine($"<p><span class='label'>Nos:</span> {note.Nose}</p>");
                    sb.AppendLine($"<p><span class='label'>Nepce:</span> {note.Palate}</p>");
                    sb.AppendLine($"<p><span class='label'>Završnica:</span> {note.Finish}</p>");
                    sb.AppendLine("</div>");
                }
            }

            sb.AppendLine($"<p style='color:#6a5840;margin-top:40px'>Generirano: {DateTime.Now:dd.MM.yyyy HH:mm}</p>");
            sb.AppendLine("</body></html>");

            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }

        public void GenerateCellarReport(WineCellar cellar, string path)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang='hr'><head><meta charset='UTF-8'><title>Izvješće podruma</title>");
            sb.AppendLine("<style>body{font-family:Georgia,serif;background:#1a1a1a;color:#e8e0d0;padding:40px}");
            sb.AppendLine("h1,h2{color:#D4AF37}table{width:100%;border-collapse:collapse;margin-top:16px}");
            sb.AppendLine("th{background:#2a2218;color:#D4AF37;padding:10px;text-align:left}");
            sb.AppendLine("td{padding:9px;border-bottom:1px solid #3a2810}</style></head><body>");

            sb.AppendLine($"<h1>{cellar.Name}</h1>");
            sb.AppendLine($"<p>Vina: {cellar.Count} | Boca: {cellar.TotalBottles} | Vrijednost: {cellar.GetTotalValue():F2} EUR</p>");

            sb.AppendLine("<h2>Katalog vina</h2>");
            sb.AppendLine("<table><tr><th>Naziv</th><th>God.</th><th>Vrsta</th><th>Regija</th><th>Boca</th><th>Status</th></tr>");

            foreach (var wine in cellar.GetAll())
            {
                sb.AppendLine($"<tr><td>{wine.Name}</td><td>{wine.Vintage}</td><td>{wine.WineType}</td>" +
                              $"<td>{wine.Region}</td><td>{wine.Quantity}</td><td>{wine.GetMaturityStatus()}</td></tr>");
            }

            sb.AppendLine("</table>");
            sb.AppendLine($"<p style='color:#6a5840;margin-top:40px'>Generirano: {DateTime.Now:dd.MM.yyyy HH:mm}</p>");
            sb.AppendLine("</body></html>");

            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }
    }
}