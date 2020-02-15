namespace GisApi.ApiServer.GraphTypes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GisApi.ApiServer.GraphTypes.Scalars.Converters;
    using GraphQL;
    using GraphQL.Types;
    using NetTopologySuite.Geometries;

    public class AppSchema : Schema
    {
        private readonly GeometryFactory geometryFactory;

        public AppSchema(QueryObject query, MutationObject mutation, GeometryFactory geometryFactory)
        {
            this.geometryFactory = geometryFactory;

            // Tags converters
            ValueConverter.Register(typeof(Dictionary<string, string>), typeof(TagsDictionary), DictToTags<string>);
            ValueConverter.Register(typeof(Dictionary<string, object>), typeof(TagsDictionary), DictToTags<object>);
            this.RegisterValueConverter(new TagsAstValueConverter());

            // Geometry.Point converters
            ValueConverter.Register(typeof(Point), typeof(GeoJSON.Geometry), GeometryToGeoJSON);
            ValueConverter.Register(typeof(GeoJSON.Geometry), typeof(Point), GeoJSONToGeometry);
            ValueConverter.Register(typeof(Dictionary<string, object>), typeof(GeoJSON.Geometry), DictToGeometry);
            this.RegisterValueConverter(new GeometryAstValueConverter());

            Query = query;
            Mutation = mutation;
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
            if ((geoJsonInput is GeoJSON.Geometry geom)
                && Enum.TryParse<OgcGeometryType>(geom.GeometryType, true, out OgcGeometryType geometryType))
            {
                switch (geometryType)
                {
                    case OgcGeometryType.Point:
                        return geometryFactory.CreatePoint(
                            new Coordinate(Convert.ToDouble(geom.Coordinates[0]),
                                           Convert.ToDouble(geom.Coordinates[1])));
                }

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
                switch (geometryType)
                {
                    case OgcGeometryType.Point:
                        var coords = dict["coordinates"] as IList<object>;

                        return geometryFactory.CreatePoint(
                            new Coordinate(Convert.ToDouble(coords[0]),
                                           Convert.ToDouble(coords[1])));
                }
            }

            return null;
        }
    }
}
