using GisApi.ApiServer.GraphTypes;
using GisApi.ApiServer.GraphTypes.Models;
using GisApi.ApiServer.GraphTypes.Scalars;
using GisApi.DataAccessLayer.Repositories;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace GisApi.ApiServer
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddGraphTypes(this IServiceCollection services) => services
            // Add custom scalars
            .AddScoped<TagsType>()
            .AddScoped<GeometryType>()
            // Add Node types
            .AddScoped<NodeType>()
            .AddScoped<NodeInputType>()
            // Add Way types
            .AddScoped<WayType>()
            .AddScoped<WayInputType>()
            // Add WayNode types
            .AddScoped<WayNodeType>()
            .AddScoped<WayNodeInputType>()
            // Add schema
            .AddScoped<QueryObject>()
            .AddScoped<MutationObject>()
            .AddScoped<ISchema, AppSchema>();

        public static IServiceCollection AddRepositories(this IServiceCollection services) => services
            .AddScoped<IWayRepository, WayRepository>();

    }
}