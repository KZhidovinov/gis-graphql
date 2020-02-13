﻿using GisApi.ApiServer.GraphTypes.Scalars.ValueNodes;
using GraphQL.Language.AST;
using GraphQL.Types;
using NetTopologySuite.Geometries;

namespace GisApi.ApiServer.GraphTypes.Scalars.Converters
{
    public class PointAstValueConverter : IAstFromValueConverter
    {
        public IValue Convert(object value, IGraphType type)
        {
            return new PointValue((Point)value);
        }

        public bool Matches(object value, IGraphType type)
        {
            return value is Point;
        }
    }
}