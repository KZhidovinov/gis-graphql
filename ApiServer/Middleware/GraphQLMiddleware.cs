namespace GisApi.ApiServer.Middleware
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using GraphQL;
    using GraphQL.NewtonsoftJson;
    using GraphQL.Types;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class GraphQLMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDocumentWriter _writer;
        private readonly IDocumentExecuter _executor;

        public GraphQLMiddleware(RequestDelegate next, IDocumentWriter writer, IDocumentExecuter executor)
        {
            _next = next;
            _writer = writer;
            _executor = executor;
        }

        public async Task InvokeAsync(HttpContext httpContext, ISchema schema)
        {
            if (httpContext.Request.Path.StartsWithSegments("/graphql", StringComparison.InvariantCultureIgnoreCase)
                && string.Equals(httpContext.Request.Method, "POST", StringComparison.OrdinalIgnoreCase))
            {
                string body;
                using (var streamReader = new StreamReader(httpContext.Request.Body))
                {
                    body = await streamReader.ReadToEndAsync().ConfigureAwait(false);

                    var request = JsonConvert.DeserializeObject<GraphQLRequest>(body);

                    var result = await _executor.ExecuteAsync(doc =>
                    {
                        doc.Schema = schema;
                        doc.Query = request.Query;
                        doc.Inputs = request.Variables.ToInputs();
                        doc.ExposeExceptions = true;
                    }).ConfigureAwait(false);

                    var json = await _writer.WriteToStringAsync(result).ConfigureAwait(false);
                    await httpContext.Response.WriteAsync(json).ConfigureAwait(false);
                }
            }
            else
            {
                await _next(httpContext).ConfigureAwait(false);
            }
        }
    }

    public class GraphQLRequest
    {
        public string Query { get; set; }
        public JObject Variables { get; set; }
    }
}
