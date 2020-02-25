namespace GisApi.ApiServer.GraphTypes.Scalars
{
    using GisApi.ApiServer.GeoJSON;
    using GraphQL;
    using GraphQL.Language.AST;
    using GraphQL.Types;

    public class FeatureType : ScalarGraphType
    {
        public FeatureType() => this.Name = "Feature";

        public override object ParseLiteral(IValue value)
        {
            return this.ParseValue(value?.Value);
        }

        public override object ParseValue(object value)
        {
            return ValueConverter.ConvertTo(value, typeof(Feature));
        }

        public override object Serialize(object value)
        {
            return ValueConverter.ConvertTo(value, typeof(Feature));
        }
    }
}