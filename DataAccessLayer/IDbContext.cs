namespace GisApi.DataAccessLayer
{
    using GisApi.DataAccessLayer.Models;
    using Microsoft.EntityFrameworkCore;

    public interface IDbContext
    {
        DbSet<Node> Nodes { get; set; }
        DbSet<Way> Ways { get; set; }

        void SaveChanges();
    }
}
