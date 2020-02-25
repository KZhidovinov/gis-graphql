namespace GisApi.ApiServer.GraphTypes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GisApi.ApiServer.GraphTypes.Scalars.Converters;
    using GisApi.DataAccessLayer.Models;
    using GraphQL;
    using GraphQL.Types;
    using NetTopologySuite.Features;
    using NetTopologySuite.Geometries;

    public class AppSchema : Schema
    {
        private readonly GeometryFactory geometryFactory;

        public AppSchema(IServiceProvider serviceProvider, QueryObject query, MutationObject mutation, GeometryFactory geometryFactory) : base(serviceProvider)
        {
            this.geometryFactory = geometryFactory;

            // Tags converters
            ValueConverter.Register(typeof(Dictionary<string, string>), typeof(TagsDictionary), this.DictToTags<string>);
            ValueConverter.Register(typeof(Dictionary<string, object>), typeof(TagsDictionary), this.DictToTags<object>);
            this.RegisterValueConverter(new TagsAstValueConverter());

            // GeoJSON.Geometry <-> Point converters
            ValueConverter.Register(typeof(Point), typeof(GeoJSON.Geometry), this.GeometryToGeoJSON);
            ValueConverter.Register(typeof(GeoJSON.Geometry), typeof(Point), this.GeoJSONToGeometry);
            ValueConverter.Register(typeof(Dictionary<string, object>), typeof(GeoJSON.Geometry), this.DictToGeometry);
            this.RegisterValueConverter(new GeometryAstValueConverter());

            // GeoJSON.Feature <-> Feature converters
            ValueConverter.Register(typeof(Feature), typeof(GeoJSON.Feature), this.FeatureToGeoJSON);
            this.RegisterValueConverter(new FeatureAstValueConverter());

            this.Query = query;
            this.Mutation = mutation;
        }

        private object FeatureToGeoJSON(object featureInput)
        {
            if (featureInput is Feature feature)
            {
                var f = new GeoJSON.Feature
                {
                    Geometry = this.GeometryToGeoJSON(feature.Geometry) as GeoJSON.Geometry,
                    ObjectType = "Feature",
                    Properties = feature.Attributes.ToDictionary(),
                    BoundingBox = feature.BoundingBox == null || feature.BoundingBox.IsNull
                        ? null
                        : new List<double> {
                            feature.BoundingBox.MinX,
                            feature.BoundingBox.MinY,
                            feature.BoundingBox.MaxX,
                            feature.BoundingBox.MaxY
                        }
                };

                return f;
            }

            return null;
        }

        private object DictToTags<TSourceValue>(object tagsInput)
        {
            if (tagsInput is IDictionary<string, TSourceValue> dict)
            {
                return new TagsDictionary(dict.Select(pair =>
                    new KeyValuePair<string, string>(pair.Key, pair.Value.ToString())));
            }

            return null;
        }

        private object GeoJSONToGeometry(object geoJsonInput)
        {
            if ((geoJsonInput is GeoJSON.Geometry geom) && Enum.TryParse(geom.GeometryType, true, out OgcGeometryType geometryType))
            {
                // Input Coordinates should contain boxed numbers
#pragma warning disable CA1305 // Specify IFormatProvider
                switch (geometryType)
                {
                    case OgcGeometryType.Point:
                        return this.geometryFactory.CreatePoint(
                            new Coordinate(Convert.ToDouble(geom.Coordinates[0]),
                                Convert.ToDouble(geom.Coordinates[1])));
                }
#pragma warning restore CA1305 // Specify IFormatProvider
            }

            return null;
        }

        private object GeometryToGeoJSON(object geoInput)
        {
            if (geoInput is Geometry geom)
            {
                switch (geom.OgcGeometryType)
                {
                    case OgcGeometryType.Point:
                        return new GeoJSON.Geometry
                        {
                            GeometryType = geom.GeometryType,
                            Coordinates = new List<object> { geom.Coordinates[0].X, geom.Coordinates[0].Y }
                        };
                    case OgcGeometryType.LineString:
                        return new GeoJSON.Geometry
                        {
                            GeometryType = geom.GeometryType,
                            Coordinates = geom.Coordinates.Select(c => new List<object> { c.X, c.Y } as object).ToList()
                        };
                    default:
                        return null;
                }
            }

            return null;
        }

        private object DictToGeometry(object pointInput)
        {
            if ((pointInput is Dictionary<string, object> dict)
                && Enum.TryParse<OgcGeometryType>(dict["type"].ToString(), true, out OgcGeometryType geometryType))
            {
                // Input Coordinates should contain boxed numbers
#pragma warning disable CA1305 // Specify IFormatProvider
                switch (geometryType)
                {
                    case OgcGeometryType.Point:
                        var coords = dict["coordinates"] as IList<object>;

                        return this.geometryFactory.CreatePoint(
                            new Coordinate(Convert.ToDouble(coords[0]),
                                Convert.ToDouble(coords[1])));
                }
#pragma warning restore CA1305 // Specify IFormatProvider
            }

            return null;
        }
    }
}