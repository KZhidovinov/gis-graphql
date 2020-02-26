namespace GisApi.DataAccessLayer.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GisApi.DataAccessLayer.Models;
    using Microsoft.EntityFrameworkCore;

    public class WayRepository : IWayRepository
    {
        private readonly IDbContext dbContext;

        public WayRepository(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateOrUpdateNodesAsync(IEnumerable<Node> nodes, CancellationToken cancellationToken)
        {
            this.dbContext.Nodes.UpdateRange(nodes);
            await this.dbContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task CreateOrUpdateWaysAsync(IEnumerable<Way> ways, CancellationToken cancellationToken)
        {
            this.dbContext.Ways.UpdateRange(ways);
            await this.dbContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Node> GetNodeByIdAsync(long id, CancellationToken cancellationToken, bool includeWayNodes = false) =>
            await this.dbContext.Nodes
                .Where(x => x.Id == id).AsQueryable()
                .If(includeWayNodes, x => x.Include(w => w.WayNodes)
                                        .ThenInclude(wn => wn.Node))
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);

        public async Task<List<Node>> GetNodesAsync(CancellationToken cancellationToken, bool includeWayNodes = false) =>
            await this.dbContext.Nodes.AsQueryable()
                .If(includeWayNodes, x => x.Include(w => w.WayNodes)
                                        .ThenInclude(wn => wn.Node))
                .AsNoTracking()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

        public async Task<List<Way>> GetWaysAsync(CancellationToken cancellationToken,
            bool includeWayNodes = false, bool includeFeature = false) =>
            await this.dbContext.Ways.AsQueryable()
                // include WayShape to be accessible from Feature getter
                .If(includeFeature, x => x.Include(w => w.WayShape))
                .If(includeWayNodes, x => x.Include(w => w.WayNodes)
                                        .ThenInclude(wn => wn.Node))
                .AsNoTracking()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
    }
}