namespace GisApi.DataAccessLayer
{
    using GisApi.DataAccessLayer.Models;
    using Microsoft.EntityFrameworkCore;

    public interface IDbContext
    {
        DbSet<Node> Nodes { get; set; }
        DbSet<Way> Ways { get; set; }
        DbSet<WayNode> WayNodes { get; set; }

        void SaveChanges();
    }
}
