namespace GisApi.DataAccessLayer
{
    using System.Threading;
    using System.Threading.Tasks;
    using GisApi.DataAccessLayer.Models;
    using Microsoft.EntityFrameworkCore;

    public interface IDbContext
    {
        DbSet<Node> Nodes { get; set; }
        DbSet<Way> Ways { get; set; }
        DbSet<WayNode> WayNodes { get; set; }

        void SaveChanges();
        Task SaveChangesAsync();
        Task SaveChangesAsync(CancellationToken token);
    }
}