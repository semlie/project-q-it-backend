using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using CodeFirst.Models;

namespace CodeFirst
{
    // Design-time factory used by EF tools to create the DbContext when running migrations
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BDQit>
    {
        public BDQit CreateDbContext(string[] args)
        {
            var cfgBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile(Path.Combine("..", "appsettings.json"), optional: true)
                .AddJsonFile(Path.Combine("..", "server-q-it", "appsettings.json"), optional: true)
                .AddEnvironmentVariables();

            var config = cfgBuilder.Build();
            var connectionString = config.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            }

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found. Set it in appsettings.json or as the DB_CONNECTION_STRING environment variable.");

            var optionsBuilder = new DbContextOptionsBuilder<BDQit>();
            optionsBuilder.UseSqlServer(connectionString);
            return new BDQit(optionsBuilder.Options);
        }
    }
}
