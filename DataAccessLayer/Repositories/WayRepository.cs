using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GisApi.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace GisApi.DataAccessLayer.Repositories
{
    public class WayRepository : IWayRepository
    {
        private readonly IDbContext dbContext;

        public WayRepository(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void CreateOrUpdateNodes(IEnumerable<Node> nodes)
        {
            this.dbContext.Nodes.UpdateRange(nodes);
            this.dbContext.SaveChanges();
        }

        public void CreateOrUpdateWays(IEnumerable<Way> ways)
        {
            this.dbContext.Ways.UpdateRange(ways);
            this.dbContext.SaveChanges();
        }

        public IEnumerable<Node> GetNodes()
        {
            return this.dbContext.Nodes.AsNoTracking().ToList();
        }

        public async Task<List<WayNode>> GetWayNodesAsync(Node node, CancellationToken cancellationToken)
        {
            return await this.dbContext.WayNodes.Where(x => x.NodeId == node.Id)
                .AsNoTracking()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public IEnumerable<Way> GetWays()
        {
            return this.dbContext.Ways
                        .Include(w => w.WayNodes)
                        .ThenInclude(wn => wn.Node)
                        .AsNoTracking()
                        .ToList();
        }
    }
}
