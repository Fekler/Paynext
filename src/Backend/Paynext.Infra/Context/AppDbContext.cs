﻿using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Paynext.Domain.Entities;
using Paynext.Infra.Configurations;


namespace Paynext.Infra.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Installment> Installments { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public AppDbContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                base.OnConfiguring(optionsBuilder);

                // Usado dessa maneira somente para usar os migrations.
                Env.TraversePath().Load();

                string? connectString = Environment.GetEnvironmentVariable("DATABASE_URL");
                if (string.IsNullOrWhiteSpace(connectString))
                {
                    throw new InvalidOperationException("A variável de ambiente DATABASE_URL não está definida.");
                }
                optionsBuilder.UseNpgsql(connectString,
                    npgsqlOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name);
                    });
            }
        }
    }
}