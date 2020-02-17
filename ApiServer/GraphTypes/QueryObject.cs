namespace GisApi.ApiServer.GraphTypes
{
    using System.Linq;
    using GisApi.ApiServer.GraphTypes.Models;
    using GisApi.DataAccessLayer;
    using GraphQL;
    using GraphQL.Types;
    using Microsoft.EntityFrameworkCore;

    public class QueryObject : ObjectGraphType
    {
        public QueryObject(IDbContext dbContext)
        {
            this.Field<NodeType>(
                "node",
                arguments: new QueryArguments(new QueryArgument<IdGraphType> { Name = "id", Description = "The ID of the Node." }),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("id");
                    var node = dbContext.Nodes.FirstOrDefault(i => i.Id == id);
                    return node;
                });

            this.Field<NonNullGraphType<ListGraphType<NonNullGraphType<NodeType>>>>(
                "nodes",
                resolve: context =>
                {
                    var nodes = dbContext.Nodes.AsNoTracking().ToList();
                    return nodes;
                });

            this.Field<NonNullGraphType<ListGraphType<NonNullGraphType<WayType>>>>(
                "ways",
                resolve: context =>
                {
                    var ways = dbContext.Ways
                        .Include(w => w.WayNodes)
                        .ThenInclude(wn => wn.Node)
                        .AsNoTracking()
                        .ToList();

                    return ways;
                });
        }
    }
}
