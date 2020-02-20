using System.Collections.Generic;
using System.Linq;
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
            var nodes = new List<Node> { new Node { OsmId = 123 }, new Node { OsmId = 456 } };
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
    }
}
