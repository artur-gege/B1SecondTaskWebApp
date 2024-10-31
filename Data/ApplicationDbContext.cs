using B1SecondTaskWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace B1SecondTaskWebAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<FileModel> Files { get; set; }
        public DbSet<DataModel> FilesData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureDataRelationships(modelBuilder);
        }

        // Конфигурация связей между моделями
        private void ConfigureDataRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FileModel>()
            .HasMany(f => f.FilesData)
            .WithOne(d => d.File)
            .HasForeignKey(d => d.FileId);

        }

    }
}
