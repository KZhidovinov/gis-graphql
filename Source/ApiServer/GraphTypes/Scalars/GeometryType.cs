namespace GisApi.ApiServer.GraphTypes.Scalars
{
    using GisApi.ApiServer.GeoJSON;
    using GraphQL;
    using GraphQL.Language.AST;
    using GraphQL.Types;

    public class GeometryType : ScalarGraphType
    {
        public GeometryType() => this.Name = "Geometry";

        public override object ParseLiteral(IValue value)
        {
            return this.ParseValue(value?.Value);
        }

        public override object ParseValue(object value)
        {
            return ValueConverter.ConvertTo(value, typeof(Geometry));
        }

        public override object Serialize(object value)
        {
            return ValueConverter.ConvertTo(value, typeof(Geometry));
        }
    }
}