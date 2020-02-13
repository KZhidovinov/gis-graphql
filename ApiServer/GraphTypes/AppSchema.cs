﻿using GisApi.ApiServer.GraphTypes.Scalars.Converters;
using GraphQL;
using GraphQL.Types;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GisApi.ApiServer.GraphTypes
{
    public class AppSchema : Schema
    {
        private readonly GeometryFactory geometryFactory;

        public AppSchema(QueryObject query, MutationObject mutation, GeometryFactory geometryFactory)
        {
            this.geometryFactory = geometryFactory;

            // Tags converters
            ValueConverter.Register(typeof(Dictionary<string, string>), typeof(Tags), ParseTags<string>);
            ValueConverter.Register(typeof(Dictionary<string, object>), typeof(Tags), ParseTags<object>);
            this.RegisterValueConverter(new TagsAstValueConverter());

            // Geometry.Point converters
            ValueConverter.Register(typeof(Point), typeof(GeoJSON.Point), PointToGeoJSON);
            ValueConverter.Register(typeof(GeoJSON.Point), typeof(Point), GeoJSONToPoint);
            ValueConverter.Register(typeof(Dictionary<string, object>), typeof(GeoJSON.Point), DictToPoint);
            this.RegisterValueConverter(new PointAstValueConverter());

            Query = query;
            Mutation = mutation;
        }

        private object GeoJSONToPoint(object geoJsonInput)
        {
            if (geoJsonInput is GeoJSON.Point pt)
            {
                return geometryFactory.CreatePoint(new Coordinate(pt.Coordinates[0], pt.Coordinates[1]));
            }

            return null;
        }

        private object ParseTags<T>(object tagsInput)
        {
            if (tagsInput is IDictionary<string, T> dict)
            {
                return new Tags(dict.Select(pair =>
                    new KeyValuePair<string, string>(pair.Key, pair.Value.ToString())));
            }

            return null;
        }

        private object PointToGeoJSON(object pointInput)
        {
            if (pointInput is Point pt)
                return new GeoJSON.Point
                {
                    Coordinates = new double[] { pt.Coordinates[0].X, pt.Coordinates[0].Y }
                };

            return null;
        }

        private object DictToPoint(object pointInput)
        {
            if (pointInput is Dictionary<string, object> dict)
            {
                var coords = dict["coordinates"] as IList<object>;

                return geometryFactory.CreatePoint(
                    new Coordinate(Convert.ToDouble(coords[0]),
                                   Convert.ToDouble(coords[1])));
            }

            return null;
        }
    }
}