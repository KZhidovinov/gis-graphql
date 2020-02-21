using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GisApi.DataAccessLayer.Models;
using GisApi.DataAccessLayer.Repositories;
using GraphQL;
using GraphQL.NewtonsoftJson;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using NSubstitute;
using Xunit;

namespace GisApi.ApiServer.Test
{
    public class MutateTests
    {
        private readonly IWayRepository repository;
        private readonly ServiceProvider serviceProvider;
        private readonly DocumentExecuter executer = new DocumentExecuter();

        public MutateTests()
        {
            this.repository = Substitute.For<IWayRepository>();

            var services = new ServiceCollection();
            services
                .AddGeometryFactory()
                .AddScoped(_ => this.repository)
                .AddGraphTypes();

            this.serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task MutateNode_AcceptsItems_ReturnsAll_TestAsync()
        {
            // Arrange
            var inputQuery = "mutation insertNodes { node(items: [" +
                "{osmId: 123243, tags: {tag1: \"23434\", tag2: \"sdf\"}, " +
                "location: {type: Point, coordinates: [12.1, 34.2]}}, " +
                "{id: 1, osmId: 3434123243, tags: {tag1: \"234343434\", tag2: \"sdfsdf\"}, " +
                "location: {type: Point, coordinates: [56.1, 54.2]}}" +
                "]) { id osmId location tags } }";

            var expectedJson = "{\"node\":[{\"id\":0,\"osmId\":123243,\"location\":{\"GeometryType\":\"Point\"," +
                "\"Coordinates\":[12.1,34.2]},\"tags\":{\"tag1\":\"23434\",\"tag2\":\"sdf\"}}," +
                "{\"id\":1,\"osmId\":3434123243,\"location\":{\"GeometryType\":\"Point\",\"Coordinates\":[56.1,54.2]}," +
                "\"tags\":{\"tag1\":\"234343434\",\"tag2\":\"sdfsdf\"}}]}";

            // Act
            var result = await this.executer.ExecuteAsync(doc =>
            {
                doc.Schema = this.serviceProvider.GetService<ISchema>();
                doc.Query = inputQuery;
                doc.ExposeExceptions = true;
            });

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Errors);
            Assert.NotNull(result.Data);

            await this.repository.Received().CreateOrUpdateNodesAsync(
                Arg.Is<IEnumerable<Node>>(nodes => nodes.Count() == 2
                    && nodes.First().OsmId == 123243
                    && nodes.First().Location != null
                    && nodes.First().Tags != null
                    && nodes.Last().Id == 1
                ),
                Arg.Any<CancellationToken>()
            );

            var resultJson = JsonSerializer.Serialize(result.Data);
            Assert.Equal(expectedJson, resultJson);
        }

        [Fact]
        public async Task MutateNode_AcceptsItemsVariable_ReturnsAll_TestAsync()
        {
            // Arrange
            var inputQuery = "mutation insertNodes($items: [NodeInput!]!) { node(items: $items) { id osmId location tags } }";

            var variablesJson = "{\"items\": [{\"osmId\": 123243, \"tags\": {\"tag1\": \"23434\"}, " +
                "\"location\": {\"type\": \"Point\", \"coordinates\": [12.1, 34.2]}}]}";

            var expectedJson = "{\"node\":[{\"id\":0,\"osmId\":123243,\"location\":{\"GeometryType\":\"Point\"," +
                "\"Coordinates\":[12.1,34.2]},\"tags\":{\"tag1\":\"23434\"}}]}";

            // Act
            var result = await this.executer.ExecuteAsync(doc =>
            {
                doc.Schema = this.serviceProvider.GetService<ISchema>();
                doc.Query = inputQuery;
                doc.Inputs = JObject.Parse(variablesJson).ToInputs();
                doc.ExposeExceptions = true;
            });

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Errors);
            Assert.NotNull(result.Data);

            await this.repository.Received().CreateOrUpdateNodesAsync(
                Arg.Is<IEnumerable<Node>>(nodes => nodes.Count() == 1
                    && nodes.First().OsmId == 123243
                    && nodes.First().Location != null
                    && nodes.First().Tags != null
                ),
                Arg.Any<CancellationToken>()
            );

            var resultJson = JsonSerializer.Serialize(result.Data);
            Assert.Equal(expectedJson, resultJson);
        }

        [Fact]
        public async Task MutateWay_AcceptsItems_ReturnsAll_TestAsync()
        {
            // Arrange
            var inputQuery = "mutation insertWays { way(items: [{osmId: 234234, tags: {tag1:  \"val1 \"}, " +
                "wayNodes: [{role:  \"start \", wayIdx: 0, node: {osmId: 12324, location: {type:  \"Point \", " +
                "coordinates: [32.4, 23.6]}, tags: {tag2: val2}}}, {role:  \"end \", wayIdx: 1, node: {osmId: 123345424, " +
                "location: {type:  \"Point \", coordinates: [62.4, 13.6]}, tags: {tag3:  \"val5 \"}}}]}]) " +
                "{ id osmId tags wayNodes { wayIdx node { id osmId } } } }";

            var expectedJson = "{\"way\":[{\"id\":0,\"osmId\":234234,\"tags\":{\"tag1\":\"val1 \"}," +
                "\"wayNodes\":[{\"wayIdx\":0,\"node\":{\"id\":0,\"osmId\":12324}}," +
                "{\"wayIdx\":1,\"node\":{\"id\":0,\"osmId\":123345424}}]}]}";

            // Act
            var result = await this.executer.ExecuteAsync(doc =>
            {
                doc.Schema = this.serviceProvider.GetService<ISchema>();
                doc.Query = inputQuery;
                doc.ExposeExceptions = true;
            });

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Errors);
            Assert.NotNull(result.Data);

            await this.repository.Received().CreateOrUpdateWaysAsync(
                Arg.Is<IEnumerable<Way>>(ways => ways.Count() == 1
                    && ways.First().OsmId == 234234
                    && ways.First().WayNodes.Last().Node.OsmId == 123345424
                    && ways.First().Tags != null
                ),
                Arg.Any<CancellationToken>()
            );

            var resultJson = JsonSerializer.Serialize(result.Data);
            Assert.Equal(expectedJson, resultJson);
        }

        [Fact]
        public async Task MutateWay_AcceptsItemsVariable_ReturnsAll_TestAsync()
        {
            // Arrange
            var inputQuery = "mutation insertWays($items: [WayInput!]!) " +
                "{ way(items: $items) { id osmId tags wayNodes { wayIdx node { id osmId } } } }";

            var variablesJson = "{\"items\":[{\"osmId\":234234,\"tags\":{\"tag1\":\"val1\"},\"wayNodes\":[{\"role\":\"start\"," +
                "\"wayIdx\":0,\"node\":{\"osmId\":12324,\"location\":{\"type\":\"Point\",\"coordinates\":[32.4,23.6]}," +
                "\"tags\":{\"tag2\":\"val2\"}}},{\"role\":\"end\",\"wayIdx\":1,\"node\":{\"osmId\":123345424," +
                "\"location\":{\"type\":\"Point\",\"coordinates\":[62.4,13.6]},\"tags\":{\"tag3\":\"val5\"}}}]}]}";

            var expectedJson = "{\"way\":[{\"id\":0,\"osmId\":234234,\"tags\":{\"tag1\":\"val1\"},\"wayNodes\":[{\"wayIdx\":0," +
                "\"node\":{\"id\":0,\"osmId\":12324}},{\"wayIdx\":1,\"node\":{\"id\":0,\"osmId\":123345424}}]}]}";

            // Act
            var result = await this.executer.ExecuteAsync(doc =>
            {
                doc.Schema = this.serviceProvider.GetService<ISchema>();
                doc.Query = inputQuery;
                doc.Inputs = JObject.Parse(variablesJson).ToInputs();
                doc.ExposeExceptions = true;
            });

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Errors);
            Assert.NotNull(result.Data);

            await this.repository.Received().CreateOrUpdateWaysAsync(
                Arg.Is<IEnumerable<Way>>(ways => ways.Count() == 1
                    && ways.First().OsmId == 234234
                    && ways.First().WayNodes.Last().Node.OsmId == 123345424
                    && ways.First().Tags != null
                ),
                Arg.Any<CancellationToken>()
            );

            var resultJson = JsonSerializer.Serialize(result.Data);
            Assert.Equal(expectedJson, resultJson);
        }
    }
}
