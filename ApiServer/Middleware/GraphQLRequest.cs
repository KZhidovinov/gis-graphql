using Newtonsoft.Json.Linq;

namespace GisApi.ApiServer.Middleware
{
    public class GraphQLRequest
    {
        public string Query { get; set; }
        public JObject Variables { get; set; }
    }
}
