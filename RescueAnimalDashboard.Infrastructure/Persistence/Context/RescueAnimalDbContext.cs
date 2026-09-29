using CsvHelper;
using Microsoft.EntityFrameworkCore;
using RescueAnimalDashboard.Domain.ValueObjects;
using RescueAnimalDashboard.Infrastructure.Persistence.Entities;
using RescueAnimalDashboard.Infrastructure.Persistence.Seeding;
using RescueAnimalDashboard.Infrastructure.Utilities;

namespace RescueAnimalDashboard.Infrastructure.Persistence.Context
{
    public class RescueAnimalDbContext(DbContextOptions<RescueAnimalDbContext> options) : DbContext(options)
    {
        private const string CSV_PATH = "Resources\\aac_shelter_outcomes.csv";
        public DbSet<AnimalRecord> Animals => Set<AnimalRecord>();
        public DbSet<AnimalTypeRecord> AnimalTypes => Set<AnimalTypeRecord>();
        public DbSet<SexUponOutcomeRecord> SexUponOutcomes => Set<SexUponOutcomeRecord>();
        public DbSet<OutcomeTypeRecord> OutcomeTypes => Set<OutcomeTypeRecord>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Seed data from CSV when database is created
            optionsBuilder
                .UseSeeding((context, _) => SeedAnimals((RescueAnimalDbContext)context))
                .UseAsyncSeeding((context, _, ct) => SeedAnimalsAsync((RescueAnimalDbContext)context, ct));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnimalTypeRecord>(b =>
            {
                b.ToTable("animal_types");
                b.HasIndex(x => x.Name).IsUnique();
            });

            modelBuilder.Entity<SexUponOutcomeRecord>(b =>
            {
                b.ToTable("sexes_upon_outcome");
                b.HasIndex(x => x.Name).IsUnique();
            });

            modelBuilder.Entity<OutcomeTypeRecord>(b =>
            {
                b.ToTable("outcome_types");
                b.HasIndex(x => x.Name).IsUnique();
            });

            modelBuilder.Entity<AnimalRecord>(b =>
            {
                b.ToTable("animals");

                // Create indexes for some columns used in rescue animal filtering
                b.HasIndex(a => a.AnimalId);
                b.HasIndex(a => a.Breed);
                b.Property(a => a.DateTime).HasColumnType("timestamp without time zone");

                // DeleteBehavior.Restrict keeps lookup values that are still in use from being deleted
                b.HasOne(a => a.AnimalType).WithMany()
                    .HasForeignKey(a => a.AnimalTypeId).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(a => a.SexUponOutcome).WithMany()
                    .HasForeignKey(a => a.SexUponOutcomeId).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(a => a.OutcomeType).WithMany()
                    .HasForeignKey(a => a.OutcomeTypeId).OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void SeedAnimals(RescueAnimalDbContext db)
        {
            if (db.Animals.Any()) return;

            var csvPath = Path.Combine(AppContext.BaseDirectory, CSV_PATH);

            db.Animals.AddRange(AnimalSeedData.Load(csvPath));
            db.SaveChanges();
        }

        private static async Task SeedAnimalsAsync(RescueAnimalDbContext db, CancellationToken ct)
        {
            if (await db.Animals.AnyAsync(ct)) return;

            var csvPath = Path.Combine(AppContext.BaseDirectory, CSV_PATH);

            db.Animals.AddRange(AnimalSeedData.Load(csvPath));
            await db.SaveChangesAsync(ct);
        }
    }
}
