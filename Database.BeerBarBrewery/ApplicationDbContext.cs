using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.BeerBarBrewery
{
    /// <summary>
    /// Represents the Entity Framework Core database context for the application.
    /// Manages entity sets and configures relationships between entities.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class using specified options.
        /// </summary>
        /// <param name="options">The options to be used by the DbContext.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        /// <summary>
        /// Gets or sets the Beer entities.
        /// </summary>
        public DbSet<Beer> Beers { get; set; }

        /// <summary>
        /// Gets or sets the Brewery entities.
        /// </summary>
        public DbSet<Brewery> Breweries { get; set; }

        /// <summary>
        /// Gets or sets the Bar entities.
        /// </summary>
        public DbSet<Bar> Bars { get; set; }

        /// <summary>
        /// Gets or sets the BarBeer join entities representing beers served at bars.
        /// </summary>
        public DbSet<BarBeer> BarBeers { get; set; }

        /// <summary>
        /// Configures the entity relationships and composite keys.
        /// </summary>
        /// <param name="modelBuilder">Provides a simple API for configuring EF Core model behavior.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure composite primary key for BarBeer join table
            modelBuilder.Entity<BarBeer>()
                .HasKey(bb => new { bb.BarId, bb.BeerId });

            // Configure relationship: BarBeer -> Bar (many-to-one)
            modelBuilder.Entity<BarBeer>()
                .HasOne(bb => bb.Bar)
                .WithMany(b => b.BarBeers)
                .HasForeignKey(bb => bb.BarId);

            // Configure relationship: BarBeer -> Beer (many-to-one)
            modelBuilder.Entity<BarBeer>()
                .HasOne(bb => bb.Beer)
                .WithMany(b => b.BarBeers)
                .HasForeignKey(bb => bb.BeerId);

            // Configure relationship: Beer -> Brewery (many-to-one)
            modelBuilder.Entity<Beer>()
                .HasOne(b => b.Brewery)
                .WithMany(br => br.Beers)
                .HasForeignKey(b => b.BreweryId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure string length constraints
            modelBuilder.Entity<Beer>()
                .Property(b => b.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Bar>()
                .Property(b => b.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Bar>()
                .Property(b => b.Address)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<Brewery>()
                .Property(b => b.Name)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
