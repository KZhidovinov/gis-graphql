using GisApi.ApiServer.Types.Models;
using GisApi.DataAccessLayer;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GisApi.ApiServer.Types
{
    public class NodeQuery : ObjectGraphType
    {
        public NodeQuery(IDbContext dbContext)
        {
            Field<NodeType>(
                "node",
                arguments: new QueryArguments(new QueryArgument<IdGraphType> { Name = "id", Description = "The ID of the Node." }),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("id");
                    var node = dbContext.Nodes.FirstOrDefault(i => i.Id == id);
                    return node;
                });

            Field<ListGraphType<NodeType>>(
                "nodes",
                resolve: context =>
                {
                    var nodes = dbContext.Nodes.AsNoTracking().ToList();
                    return nodes;
                });
        }
    }
}
