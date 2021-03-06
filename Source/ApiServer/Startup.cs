namespace GisApi.ApiServer
{
    using GisApi.ApiServer.Middleware;
    using GisApi.DataAccessLayer;
    using GraphiQl;
    using GraphQL;
    using GraphQL.NewtonsoftJson;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Rewrite;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) => services
            .AddApplicationInsightsTelemetry()
            // Add GraphQL writers
            .AddSingleton<IDocumentWriter, DocumentWriter>()
            .AddSingleton<IDocumentExecuter, DocumentExecuter>()
            // Initialize Geometry Factory
            .AddGeometryFactory()
            .AddDbContext<IDbContext, SqlServerDbContext>()
            .AddRepositories()
            .AddGraphTypes();

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) =>
            app.If(env.IsDevelopment(), app => app.UseDeveloperExceptionPage())
            .UseDefaultFiles()
            .UseStaticFiles()
            .UseGraphiQl("/graphiql", "/graphql")
            // ToDo: remove line below when root route is ready
            .UseRewriter(new RewriteOptions().AddRedirect("^$", "/graphiql"))
            .UseMiddleware<GraphQLMiddleware>();
    }
}