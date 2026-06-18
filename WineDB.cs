using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;


namespace WineCollector
{
    public class WineDB : DbContext
    {
        public WineDB() : base("name=WineCollector") { }

        public DbSet<RedWine> RedWines { get; set; }
        public DbSet<WhiteWine> WhiteWines { get; set; }
        public DbSet<RoseWine> RoseWines { get; set; }
        public DbSet<SparklingWine> SparklingWines { get; set; }
        public DbSet<DessertWine> DessertWines { get; set; }
        public DbSet<FortifiedWine> FortifiedWines { get; set; }
        public DbSet<TastingNote> TastingNotes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RedWine>().ToTable("RedWines");
            modelBuilder.Entity<WhiteWine>().ToTable("WhiteWines");
            modelBuilder.Entity<RoseWine>().ToTable("RoseWines");
            modelBuilder.Entity<SparklingWine>().ToTable("SparklingWines");
            modelBuilder.Entity<DessertWine>().ToTable("DessertWines");
            modelBuilder.Entity<FortifiedWine>().ToTable("FortifiedWines");
        }
    }
}