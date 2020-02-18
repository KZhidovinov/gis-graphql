using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GisApi.DataAccessLayer.Models;

namespace GisApi.DataAccessLayer.Repositories
{
    public interface IWayRepository
    {
        IEnumerable<Way> GetWays();
        void CreateOrUpdateWays(IEnumerable<Way> ways);

        IEnumerable<Node> GetNodes();
        void CreateOrUpdateNodes(IEnumerable<Node> nodes);
        Task<List<WayNode>> GetWayNodesAsync(Node source, CancellationToken cancellationToken);
    }
}
