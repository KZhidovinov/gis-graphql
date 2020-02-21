namespace GisApi.ApiServer.Middleware
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using GraphQL;
    using GraphQL.NewtonsoftJson;
    using GraphQL.Types;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Hosting;
    using Newtonsoft.Json;

    public class GraphQLMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IDocumentWriter writer;
        private readonly IDocumentExecuter executor;

        public GraphQLMiddleware(RequestDelegate next, IDocumentWriter writer, IDocumentExecuter executor)
        {
            this.next = next;
            this.writer = writer;
            this.executor = executor;
        }

        public async Task InvokeAsync(HttpContext httpContext, ISchema schema, IHostEnvironment env)
        {
            if (httpContext.Request.Path.StartsWithSegments("/graphql", StringComparison.InvariantCultureIgnoreCase)
                && string.Equals(httpContext.Request.Method, "POST", StringComparison.OrdinalIgnoreCase))
            {
                string body;
                using (var streamReader = new StreamReader(httpContext.Request.Body))
                {
                    body = await streamReader.ReadToEndAsync().ConfigureAwait(false);

                    var request = JsonConvert.DeserializeObject<GraphQLRequest>(body);

                    var result = await this.executor.ExecuteAsync(doc =>
                    {
                        doc.Schema = schema;
                        doc.Query = request.Query;
                        doc.Inputs = request.Variables.ToInputs();

                        if (env.IsDevelopment())
                        {
                            doc.ExposeExceptions = true;
                        }
                    }).ConfigureAwait(false);

                    var json = await this.writer.WriteToStringAsync(result).ConfigureAwait(false);
                    await httpContext.Response.WriteAsync(json).ConfigureAwait(false);
                }
            }
            else
            {
                await this.next(httpContext).ConfigureAwait(false);
            }
        }
    }
}