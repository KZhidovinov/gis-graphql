namespace GisApi.ApiServer.GraphTypes.Scalars
{
    using GisApi.ApiServer.GeoJSON;
    using GraphQL;
    using GraphQL.Language.AST;
    using GraphQL.Types;

    public class PointType : ScalarGraphType
    {
        public PointType() => Name = "Point";

        public override object ParseLiteral(IValue value)
        {
            return ParseValue(value.Value);
        }

        public override object ParseValue(object value)
        {
            return ValueConverter.ConvertTo(value, typeof(Point));
        }

        public override object Serialize(object value)
        {
            return ValueConverter.ConvertTo(value, typeof(Point));
        }
    }
}
