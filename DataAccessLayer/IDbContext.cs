using GisApi.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace GisApi.DataAccessLayer
{
    public interface IDbContext
    {
        DbSet<Node> Nodes { get; set; }
        DbSet<Way> Ways { get; set; }

        void SaveChanges();
    }
}
