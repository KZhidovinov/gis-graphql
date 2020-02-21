using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GisApi.DataAccessLayer.Models;
using GisApi.DataAccessLayer.Repositories;
using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using NetTopologySuite.Geometries;
using NSubstitute;
using Xunit;

namespace GisApi.ApiServer.Test
{
    public class QueryNodesTests
    {
        private readonly IWayRepository repository;
        private readonly ServiceProvider serviceProvider;
        private readonly DocumentExecuter executer = new DocumentExecuter();

        public QueryNodesTests()
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
        public async Task QueryNodes_ReturnsId_TestAsync()
        {
            // Arrange
            var nodes = new List<Node> {
                new Node { Id = 123 },
                new Node { Id = 456 }
            };
            this.repository.GetNodesAsync(Arg.Any<CancellationToken>()).Returns(nodes);

            var expected = new Dictionary<string, object>
            {
                ["nodes"] = nodes.Select(n => new Dictionary<string, object> { ["id"] = n.Id }).ToList()
            };

            // Act
            var result = await this.executer.ExecuteAsync(doc =>
            {
                doc.Schema = this.serviceProvider.GetService<ISchema>();
                doc.Query = "{ nodes { id } }";
                doc.ExposeExceptions = true;
            });

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Errors);
            Assert.NotNull(result.Data);

            Assert.Equal(expected, result.Data);
        }

        [Fact]
        public async Task QueryNodes_ReturnsOsmId_TestAsync()
        {
            // Arrange
            var nodes = new List<Node> { new Node { OsmId = 123 }, new Node { OsmId = long.MaxValue } };
            this.repository.GetNodesAsync(Arg.Any<CancellationToken>()).Returns(nodes);

            var expected = new Dictionary<string, object>
            {
                ["nodes"] = nodes.Select(n => new Dictionary<string, object> { ["osmId"] = n.OsmId }).ToList()
            };

            // Act
            var result = await this.executer.ExecuteAsync(doc =>
            {
                doc.Schema = this.serviceProvider.GetService<ISchema>();
                doc.Query = "{ nodes { osmId } }";
                doc.ExposeExceptions = true;
            });

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Errors);
            Assert.NotNull(result.Data);

            Assert.Equal(expected, result.Data);
        }

        [Fact]
        public async Task QueryNodes_ReturnsLocation_TestAsync()
        {
            // Arrange
            var nodes = new List<Node> {
                new Node { Location = this.serviceProvider.GetService<GeometryFactory>().CreatePoint(new Coordinate(12.23, 23.44)) },
                new Node { Location = this.serviceProvider.GetService<GeometryFactory>().CreatePoint(new Coordinate(21.43, 45.77)) }
            };
            this.repository.GetNodesAsync(Arg.Any<CancellationToken>()).Returns(nodes);
            var expectedJson = "{\"nodes\":[{\"location\":{\"GeometryType\":\"Point\",\"Coordinates\":[12.23,23.44]}}," +
                "{\"location\":{\"GeometryType\":\"Point\",\"Coordinates\":[21.43,45.77]}}]}";

            // Act
            var result = await this.executer.ExecuteAsync(doc =>
            {
                doc.Schema = this.serviceProvider.GetService<ISchema>();
                doc.Query = "{ nodes { location } }";
                doc.ExposeExceptions = true;
            });

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Errors);
            Assert.NotNull(result.Data);

            var resultJson = JsonSerializer.Serialize(result.Data);
            Assert.Equal(expectedJson, resultJson);
        }

        [Fact]
        public async Task QueryNodes_ReturnsTags_TestAsync()
        {
            // Arrange
            var nodes = new List<Node>
            {
                new Node { Tags = new TagsDictionary { ["tag1"] = "val1", ["tag2"] = "val2" } },
                new Node { Tags = new TagsDictionary { ["tag3"] = "val3", ["tag2"] = "val4" } }
            };
            this.repository.GetNodesAsync(Arg.Any<CancellationToken>()).Returns(nodes);

            var expected = new Dictionary<string, object>
            {
                ["nodes"] = nodes.Select(n => new Dictionary<string, object> { ["tags"] = n.Tags }).ToList()
            };

            // Act
            var result = await this.executer.ExecuteAsync(doc =>
            {
                doc.Schema = this.serviceProvider.GetService<ISchema>();
                doc.Query = "{ nodes { tags } }";
                doc.ExposeExceptions = true;
            });

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Errors);
            Assert.NotNull(result.Data);

            Assert.Equal(expected, result.Data);
        }

        [Fact]
        public async Task QueryNodes_ReturnsWayNodesWayId_TestAsync()
        {
            // Arrange
            var way = new Way { Id = 5 };
            var nodes = new List<Node> {
                new Node { Id = 1, WayNodes = new List<WayNode>{ new WayNode { Way = way } } },
                new Node { Id = 2, WayNodes = new List<WayNode>{ new WayNode { Way = way } } }
            };

            this.repository.GetNodesAsync(Arg.Any<CancellationToken>()).Returns(nodes);

            var expectedJson = "{\"nodes\":[{\"wayNodes\":[{\"way\":{\"id\":5}}]},{\"wayNodes\":[{\"way\":{\"id\":5}}]}]}";

            // Act
            var result = await this.executer.ExecuteAsync(doc =>
            {
                doc.Schema = this.serviceProvider.GetService<ISchema>();
                doc.Query = "{ nodes { wayNodes { way { id } } } }";
                doc.ExposeExceptions = true;
            });

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Errors);
            Assert.NotNull(result.Data);

            var resultJson = JsonSerializer.Serialize(result.Data);
            Assert.Equal(expectedJson, resultJson);
        }

        [Fact]
        public async Task QueryNodeById_ReturnsIdOsmId_TestAsync()
        {
            // Arrange
            var node = new Node { Id = 1, OsmId = long.MaxValue };
            this.repository.GetNodeByIdAsync(node.Id).Returns(node);
            var expectedJson = "{\"node\":{\"id\":1,\"osmId\":9223372036854775807}}";

            // Act
            var result = await this.executer.ExecuteAsync(doc =>
            {
                doc.Schema = this.serviceProvider.GetService<ISchema>();
                doc.Query = "{ node(id: 1) { id osmId } }";
                doc.ExposeExceptions = true;
            });

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Errors);
            Assert.NotNull(result.Data);

            var resultJson = JsonSerializer.Serialize(result.Data);
            Assert.Equal(expectedJson, resultJson);
        }

        [Fact]
        public async Task QueryNodeByIdVariable_ReturnsIdOsmId_TestAsync()
        {
            // Arrange
            var node = new Node { Id = 1, OsmId = long.MaxValue };
            this.repository.GetNodeByIdAsync(node.Id).Returns(node);
            var expectedJson = "{\"node\":{\"id\":1,\"osmId\":9223372036854775807}}";

            // Act
            var result = await this.executer.ExecuteAsync(doc =>
            {
                doc.Schema = this.serviceProvider.GetService<ISchema>();
                doc.Query = "query nodeById($id: ID!){ node(id: $id) { id osmId } }";
                doc.Inputs = new Inputs(new Dictionary<string, object> { ["id"] = 1 });
                doc.ExposeExceptions = true;
            });

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Errors);
            Assert.NotNull(result.Data);

            var resultJson = JsonSerializer.Serialize(result.Data);
            Assert.Equal(expectedJson, resultJson);
        }
    }
}
