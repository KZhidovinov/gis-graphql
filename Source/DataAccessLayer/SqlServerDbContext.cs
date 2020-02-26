namespace GisApi.DataAccessLayer
{
    using System.Linq;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using GisApi.DataAccessLayer.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using Microsoft.Extensions.Configuration;

    public class SqlServerDbContext : DbContext, IDbContext
    {
        public IConfiguration Configuration { get; }

        public virtual DbSet<Node> Nodes { get; set; }
        public virtual DbSet<Way> Ways { get; set; }
        public virtual DbSet<WayNode> WayNodes { get; set; }
        public virtual DbSet<WayShape> WayShapes { get; set; }

        public SqlServerDbContext() : base() { }
        public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options, IConfiguration configuration) : base(options)
        {
            this.Configuration = configuration;
        }

        public SqlServerDbContext(IConfiguration configuration) : base()
        {
            this.Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = this.Configuration.GetConnectionString("gis_api");
                optionsBuilder.UseSqlServer(connectionString, x => x.UseNetTopologySuite());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var tagsConverter = new ValueConverter<TagsDictionary, string>(
                    v => JsonSerializer.Serialize(v, new JsonSerializerOptions { IgnoreNullValues = true }),
                    v => JsonSerializer.Deserialize<TagsDictionary>(v, new JsonSerializerOptions { IgnoreNullValues = true }));

            var tagsComparer = new ValueComparer<TagsDictionary>(
                (t1, t2) => t1.All(p => t2.ContainsKey(p.Key) && p.Value.Equals(t2[p.Key]))
                    && t2.All(p => t1.ContainsKey(p.Key) && p.Value.Equals(t1[p.Key])),
                tags => tags.GetHashCode());

            modelBuilder.Entity<Node>(b =>
            {
                b.Property(e => e.Tags)
                    .HasConversion(tagsConverter)
                    .Metadata.SetValueComparer(tagsComparer);

                b.Property(e => e.Location).HasColumnType("geometry");
                b.HasMany(e => e.WayNodes).WithOne();
            });

            modelBuilder.Entity<Way>(b =>
            {
                b.Property(e => e.Tags)
                    .HasConversion(tagsConverter)
                    .Metadata.SetValueComparer(tagsComparer);

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

            modelBuilder.Entity<WayShape>(b =>
            {
                // View receives WayId from Way.Id so it must be unique
                b.ToView("WayShapes").HasKey(e => e.WayId);

                b.HasOne(ws => ws.Way)
                    .WithOne(w => w.WayShape);
            });
        }

        void IDbContext.SaveChanges()
        {
            this.SaveChanges();
        }

        async Task IDbContext.SaveChangesAsync()
        {
            await this.SaveChangesAsync().ConfigureAwait(false);
        }

        async Task IDbContext.SaveChangesAsync(CancellationToken cancellationToken)
        {
            await this.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

    }
}