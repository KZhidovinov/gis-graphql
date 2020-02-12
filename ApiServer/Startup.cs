using GisApi.ApiServer.Middleware;
using GisApi.ApiServer.Types;
using GisApi.ApiServer.Types.Models;
using GisApi.DataAccessLayer;
using GraphiQl;
using GraphQL;
using GraphQL.Http;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiServer
{
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

            services.AddSingleton<TagsType>();

            services.AddEntityFrameworkSqlServer()
                .AddDbContext<IDbContext, SqlServerDbContext>(opts => opts
                     .UseSqlServer(this.Configuration.GetConnectionString("gis_api"),
                         x => x.UseNetTopologySuite()));

            services.AddTransient<NodeQuery>();
            services.AddTransient<NodeMutation>();
            services.AddTransient<ISchema, NodeSchema>();
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
