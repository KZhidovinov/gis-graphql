using GisApi.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Text.Json;

namespace GisApi.DataAccessLayer
{
    public class SqlServerDbContext : DbContext, IDbContext
    {
        public IConfiguration Configuration { get; }

        public virtual DbSet<Node> Nodes { get; set; }
        public virtual DbSet<Way> Ways { get; set; }
        public virtual DbSet<WayNode> WayNodes { get; set; }

        public SqlServerDbContext() : base() { }

        public SqlServerDbContext(IConfiguration configuration) : base()
        {
            this.Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("server=.\\mssql2017;database=gis_api;uid=sa;pwd=1;",
                    x => x.UseNetTopologySuite());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Node>(b =>
            {
                b.Property(e => e.Tags)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, new JsonSerializerOptions { IgnoreNullValues = true }),
                        v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, new JsonSerializerOptions { IgnoreNullValues = true })
                    );
                b.Property(e => e.Location)
                    .HasColumnType("geometry");

                b.HasMany(e => e.WayNodes).WithOne();
            });


            modelBuilder.Entity<Way>(b =>
            {
                b.Property(e => e.Tags)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, new JsonSerializerOptions { IgnoreNullValues = true }),
                        v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, new JsonSerializerOptions { IgnoreNullValues = true })
                    );
                b.HasMany(e => e.WayNodes).WithOne();
            });

            modelBuilder.Entity<WayNode>(b =>
            {
                b.HasKey(new[] { "WayId", "NodeId" }).IsClustered();
                b.HasOne(wn => wn.Way)
                    .WithMany(w => w.WayNodes)
                    .HasForeignKey(wn => wn.WayId)
                    .HasPrincipalKey(w => w.Id);

                b.HasOne(wn => wn.Node)
                    .WithMany(n => n.WayNodes)
                    .HasForeignKey(wn => wn.NodeId)
                    .HasPrincipalKey(n => n.Id);
            });
        }

        void IDbContext.SaveChanges()
        {
            this.SaveChanges();
        }

    }
}