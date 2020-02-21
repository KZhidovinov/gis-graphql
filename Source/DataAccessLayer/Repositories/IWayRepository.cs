namespace GisApi.DataAccessLayer.Repositories
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using GisApi.DataAccessLayer.Models;
    using NetTopologySuite.Geometries;

    public interface IWayRepository
    {
        Task CreateOrUpdateWaysAsync(IEnumerable<Way> ways, CancellationToken cancellationToken);
        Task CreateOrUpdateNodesAsync(IEnumerable<Node> nodes, CancellationToken cancellationToken);
        Task<List<Node>> GetNodesAsync(CancellationToken cancellationToken);
        Task<List<Node>> GetNodesAsync(Way way, CancellationToken cancellationToken);
        Task<List<Way>> GetWaysAsync(CancellationToken cancellationToken);
        Task<Node> GetNodeByIdAsync(long id);
        Task<List<WayNode>> GetWayNodesAsync(Node source, CancellationToken cancellationToken);
        Task<LineString> GetWayShape(Way way, CancellationToken cancellationToken);
    }
}