namespace GisApi.DataAccessLayer.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GisApi.DataAccessLayer.Models;
    using Microsoft.EntityFrameworkCore;
    using NetTopologySuite.Geometries;

    public class WayRepository : IWayRepository
    {
        private readonly IDbContext dbContext;
        private readonly GeometryFactory geometryFactory;

        public WayRepository(IDbContext dbContext, GeometryFactory geometryFactory)
        {
            this.dbContext = dbContext;
            this.geometryFactory = geometryFactory;
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

        public async Task<Node> GetNodeByIdAsync(long id)
        {
            return await this.dbContext.Nodes
                .Where(x => x.Id == id)
                .Include(x => x.WayNodes)
                .ThenInclude(x => x.Way)
                .AsNoTracking()
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);
        }

        public async Task<List<Node>> GetNodesAsync(CancellationToken cancellationToken) =>
            await this.dbContext.Nodes
            .Include(x => x.WayNodes)
            .ThenInclude(x => x.Way)
            .AsNoTracking()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        public async Task<List<Node>> GetNodesAsync(Way way, CancellationToken cancellationToken) =>
            await this.dbContext.WayNodes
            .Where(x => x.WayId == way.Id)
            .OrderBy(x => x.WayIdx)
            .Select(x => x.Node)
            .AsNoTracking()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        public async Task<List<WayNode>> GetWayNodesAsync(Node node, CancellationToken cancellationToken) =>
            await this.dbContext.WayNodes
            .Where(x => x.NodeId == node.Id)
            .AsNoTracking()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        public async Task<List<Way>> GetWaysAsync(CancellationToken cancellationToken) =>
            await this.dbContext.Ways
            .Include(w => w.WayNodes)
            .ThenInclude(wn => wn.Node)
            .AsNoTracking()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        public async Task<LineString> GetWayShape(Way way, CancellationToken cancellationToken) =>
            this.geometryFactory.CreateLineString(
                await this.dbContext.WayNodes
                .Where(x => x.WayId == way.Id)
                .OrderBy(x => x.WayIdx)
                .Select(x => x.Node.Location.Coordinate)
                .ToArrayAsync(cancellationToken)
                .ConfigureAwait(false));
    }
}