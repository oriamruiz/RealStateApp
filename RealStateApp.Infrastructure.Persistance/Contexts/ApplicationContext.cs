using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Domain.Entities;


namespace RealStateApp.Infrastructure.Persistance.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
    
        public DbSet<PropertyImprovement> PropertyImprovements { get; set; }
        public DbSet<Improvement> Improvements { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<SaleType> SaleTypes { get; set; }

        public DbSet<FavoriteProperty> FavoriteProperties { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Tables
            modelBuilder.Entity<PropertyImprovement>().ToTable("PropertyImprovements");
            modelBuilder.Entity<Improvement>().ToTable("Improvements");
            modelBuilder.Entity<Property>().ToTable("Properties");
            modelBuilder.Entity<PropertyType>().ToTable("PropertyTypes");
            modelBuilder.Entity<SaleType>().ToTable("SaleTypes");
            modelBuilder.Entity<FavoriteProperty>().ToTable("FavoriteProperties");

            #endregion

            #region PrimaryKeys
            modelBuilder.Entity<PropertyImprovement>().HasKey(u => u.Id);
            modelBuilder.Entity<Improvement>().HasKey(f => f.Id);
            modelBuilder.Entity<Property>().HasKey(p => p.Id);
            modelBuilder.Entity<PropertyType>().HasKey(c => c.Id);
            modelBuilder.Entity<SaleType>().HasKey(r => r.Id);
            modelBuilder.Entity<FavoriteProperty>().HasKey(r => r.Id);

            #endregion

            #region Relationships
            modelBuilder.Entity<Property>()
                .HasMany<PropertyImprovement>(c => c.Improvements)
                .WithOne(a => a.Property)
                .HasForeignKey(a => a.PropertyId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PropertyImprovement>()
                .HasOne(t => t.Improvement)
                .WithMany()
                .HasForeignKey(t => t.ImprovementId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FavoriteProperty>()
                .HasOne(t => t.Property)
                .WithMany()
                .HasForeignKey(t => t.PropertyId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PropertyType>()
               .HasMany<Property>(c => c.Properties)
               .WithOne(a => a.PropertyType)
               .HasForeignKey(a => a.PropertyTypeId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SaleType>()
               .HasMany<Property>(c => c.Properties)
               .WithOne(a => a.SaleType)
               .HasForeignKey(a => a.SaleTypeId)
               .OnDelete(DeleteBehavior.NoAction);


            #endregion

            #region Property Confirguration


            #region Property
            modelBuilder.Entity<Property>()
                .HasIndex(p => p.Code)
                .IsUnique();

            modelBuilder.Entity<Property>()
                .Property(p => p.Price)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Property>()
                .Property(p => p.Description)
                .HasMaxLength(int.MaxValue);


            #endregion

            #region FavoriteProperty
            modelBuilder.Entity<FavoriteProperty>()
                .HasIndex(f => new { f.ClientId, f.PropertyId })
                .IsUnique();


            #endregion


            #region PropertyImprovement
            modelBuilder.Entity<PropertyImprovement>()
                .HasIndex(f => new { f.PropertyId, f.ImprovementId })
                .IsUnique();


            #endregion

            #region Improvement
            modelBuilder.Entity<Improvement>()
                .Property(p => p.Description)
                .HasMaxLength(int.MaxValue);

            modelBuilder.Entity<Improvement>()
                .Property(p => p.Name)
                .HasMaxLength(100);

            modelBuilder.Entity<Improvement>()
                .HasIndex(u => u.Name)
                .IsUnique();

            #endregion


            #region PropertyType
            modelBuilder.Entity<PropertyType>()
                .Property(p => p.Description)
                .HasMaxLength(int.MaxValue);

            modelBuilder.Entity<PropertyType>()
                .Property(p => p.Name)
                .HasMaxLength(100);

            modelBuilder.Entity<PropertyType>()
                .HasIndex(u => u.Name)
                .IsUnique();
            #endregion

            #region SaleType
            modelBuilder.Entity<SaleType>()
                .Property(p => p.Description)
                .HasMaxLength(int.MaxValue);

            modelBuilder.Entity<SaleType>()
                .Property(p => p.Name)
                .HasMaxLength(100);

            modelBuilder.Entity<SaleType>()
                .HasIndex(u => u.Name)
                .IsUnique();
            #endregion




            #endregion

        }
    }
}
