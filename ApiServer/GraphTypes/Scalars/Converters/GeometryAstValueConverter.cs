namespace GisApi.ApiServer.GraphTypes.Scalars.Converters
{
    using GisApi.ApiServer.GraphTypes.Scalars.ValueNodes;
    using GraphQL.Language.AST;
    using GraphQL.Types;
    using NetTopologySuite.Geometries;

    public class GeometryAstValueConverter : IAstFromValueConverter
    {
        public IValue Convert(object value, IGraphType type)
        {
            return new GeometryValue((Point)value);
        }

        public bool Matches(object value, IGraphType type)
        {
            return value is Point;
        }
    }
}