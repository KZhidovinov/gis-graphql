namespace GisApi.DataAccessLayer.Repositories
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using GisApi.DataAccessLayer.Models;
    using NetTopologySuite.Features;

    public interface IWayRepository
    {
        Task CreateOrUpdateWaysAsync(IEnumerable<Way> ways, CancellationToken cancellationToken);
        Task CreateOrUpdateNodesAsync(IEnumerable<Node> nodes, CancellationToken cancellationToken);
        Task<List<Node>> GetNodesAsync(CancellationToken cancellationToken, bool includeWayNodes = false);
        Task<List<Way>> GetWaysAsync(CancellationToken cancellationToken, bool includeWayNodes = false, bool includeFeature = false);
        Task<Node> GetNodeByIdAsync(long id, CancellationToken cancellationToken, bool includeWayNodes = false);
    }
}