using System;

namespace WineCollector
{
    public class RedWine : Wine
    {
        public int TaninLevel { get; set; } = 5;
        public int AcidityLevel { get; set; } = 5;
        public bool OakAged { get; set; }
        public int OakMonths { get; set; }

        public RedWine()
        {
            WineType = WineType.Crno;
        }

        public override (int Start, int End) GetDrinkingWindow()
        {
            int start = 3 + TaninLevel / 2;
            int end = 8 + TaninLevel;

            if (OakAged)
            {
                start += OakMonths / 24;
                end += OakMonths / 12;
            }

            end += AcidityLevel / 3;

            return (Vintage + start, Vintage + end);
        }

        public override MaturityStatus GetMaturityStatus()
        {
            int year = DateTime.Now.Year;
            var (s, e) = GetDrinkingWindow();

            if (year < s - 1) return MaturityStatus.Mlado;
            if (year == s - 1) return MaturityStatus.UlazakUOptimum;
            if (year <= e - 1) return MaturityStatus.Optimalno;
            if (year <= e + 2) return MaturityStatus.Opadanje;
            return MaturityStatus.ProšaoOptimum;
        }

        public override string FormatLabel()
        {
            string base_ = base.FormatLabel();
            base_ += $"Tanini:     {TaninLevel}/10\n";
            base_ += $"Kiselost:   {AcidityLevel}/10\n";
            base_ += $"Hrast:      {(OakAged ? $"Da – {OakMonths} mj." : "Ne")}\n";
            return base_;
        }
    }

    public class WhiteWine : Wine
    {
        public double ResidualSugar { get; set; }
        public int AcidityLevel { get; set; } = 6;
        public bool MalolacticFermentation { get; set; }

        public WhiteWine()
        {
            WineType = WineType.Bijelo;
        }

        public override (int Start, int End) GetDrinkingWindow()
        {
            int start = 1;
            int end = 4;

            if (AcidityLevel >= 8) { end += 8; start += 1; }
            else if (AcidityLevel >= 6) { end += 3; }

            if (ResidualSugar > 45) { end += 10; start += 2; }
            else if (ResidualSugar > 12) { end += 4; }

            if (MalolacticFermentation) end -= 1;

            return (Vintage + start, Vintage + Math.Max(end, start + 1));
        }

        public override MaturityStatus GetMaturityStatus()
        {
            int year = DateTime.Now.Year;
            var (s, e) = GetDrinkingWindow();

            if (year < s) return MaturityStatus.Mlado;
            if (year == s) return MaturityStatus.UlazakUOptimum;
            if (year < e) return MaturityStatus.Optimalno;
            if (year == e) return MaturityStatus.Opadanje;
            return MaturityStatus.ProšaoOptimum;
        }

        public override string FormatLabel()
        {
            string base_ = base.FormatLabel();
            string sladoca = ResidualSugar < 4 ? "Suho"
                           : ResidualSugar < 12 ? "Polusuho"
                           : ResidualSugar < 45 ? "Poluslatko" : "Slatko";
            base_ += $"Šećer:      {ResidualSugar:F1} g/L ({sladoca})\n";
            base_ += $"Kiselost:   {AcidityLevel}/10\n";
            base_ += $"MLF:        {(MalolacticFermentation ? "Da" : "Ne")}\n";
            return base_;
        }
    }

    public class RoseWine : Wine
    {
        public RoseWine()
        {
            WineType = WineType.Rose;
        }

        public override (int Start, int End) GetDrinkingWindow()
        {
            return (Vintage + 1, Vintage + 3);
        }

        public override MaturityStatus GetMaturityStatus()
        {
            int year = DateTime.Now.Year;
            var (s, e) = GetDrinkingWindow();

            if (year < s) return MaturityStatus.Mlado;
            if (year <= e) return MaturityStatus.Optimalno;
            return MaturityStatus.ProšaoOptimum;
        }
    }

    public class SparklingWine : Wine
    {
        public string Method { get; set; } = "Charmat";
        public string Dosage { get; set; } = "Brut";
        public int LeeAgingMonths { get; set; } = 18;

        public SparklingWine()
        {
            WineType = WineType.Pjenušac;
        }

        public override (int Start, int End) GetDrinkingWindow()
        {
            if (Method.Contains("Champenoise") || Method.Contains("Tradicional"))
            {
                int bonus = LeeAgingMonths / 12;
                return (Vintage + 3 + bonus, Vintage + 12 + bonus);
            }
            return (Vintage + 1, Vintage + 3);
        }

        public override MaturityStatus GetMaturityStatus()
        {
            int year = DateTime.Now.Year;
            var (s, e) = GetDrinkingWindow();

            if (year < s) return MaturityStatus.Mlado;
            if (year == s) return MaturityStatus.UlazakUOptimum;
            if (year < e) return MaturityStatus.Optimalno;
            if (year == e) return MaturityStatus.Opadanje;
            return MaturityStatus.ProšaoOptimum;
        }
    }

    public class DessertWine : Wine
    {
        public double ResidualSugar { get; set; } = 150;
        public bool Botrytis { get; set; }

        public DessertWine()
        {
            WineType = WineType.Desertno;
        }

        public override (int Start, int End) GetDrinkingWindow()
        {
            int start = 5;
            int end = 20;

            if (Botrytis) { start += 3; end += 20; }
            if (ResidualSugar > 200) end += 15;
            else if (ResidualSugar > 120) end += 7;

            return (Vintage + start, Vintage + end);
        }

        public override MaturityStatus GetMaturityStatus()
        {
            int year = DateTime.Now.Year;
            var (s, e) = GetDrinkingWindow();

            if (year < s - 2) return MaturityStatus.Mlado;
            if (year < s) return MaturityStatus.UlazakUOptimum;
            if (year <= e) return MaturityStatus.Optimalno;
            return MaturityStatus.Opadanje;
        }
    }

    public class FortifiedWine : Wine
    {
        public string Style { get; set; } = "Vintage";
        public double AlcoholPercent { get; set; } = 19.5;

        public FortifiedWine()
        {
            WineType = WineType.Pojačano;
        }

        public override (int Start, int End) GetDrinkingWindow()
        {
            if (Style.Contains("Vintage")) return (Vintage + 10, Vintage + 50);
            if (Style.Contains("LBV")) return (Vintage + 4, Vintage + 20);
            if (Style.Contains("Tawny")) return (DateTime.Now.Year, DateTime.Now.Year + 5);
            return (Vintage + 2, Vintage + 15);
        }

        public override MaturityStatus GetMaturityStatus()
        {
            int year = DateTime.Now.Year;
            var (s, e) = GetDrinkingWindow();

            if (year < s) return MaturityStatus.Mlado;
            if (year <= e) return MaturityStatus.Optimalno;
            return MaturityStatus.Opadanje;
        }
    }
}