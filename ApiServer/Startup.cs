namespace GisApi.ApiServer
{
    using GisApi.ApiServer.GraphTypes;
    using GisApi.ApiServer.GraphTypes.Scalars;
    using GisApi.ApiServer.Middleware;
    using GisApi.DataAccessLayer;
    using GraphiQl;
    using GraphQL;
    using GraphQL.NewtonsoftJson;
    using GraphQL.Types;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using NetTopologySuite.Geometries;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDocumentWriter, DocumentWriter>();
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton(new GeometryFactory(new PrecisionModel(PrecisionModels.Floating), 4326));

            services.AddSingleton<TagsType>();

            services.AddEntityFrameworkSqlServer()
                .AddDbContext<IDbContext, SqlServerDbContext>();

            services.AddTransient<QueryObject>();
            services.AddTransient<MutationObject>();
            services.AddTransient<ISchema, AppSchema>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseGraphiQl("/graphiql", "/graphql");

            app.UseMiddleware<GraphQLMiddleware>();
        }
    }
}
