namespace GisApi.ApiServer.GraphTypes.Scalars.Converters
{
    using GisApi.ApiServer.GraphTypes.Scalars.ValueNodes;
    using GraphQL.Language.AST;
    using GraphQL.Types;
    using NetTopologySuite.Features;

    public class FeatureAstValueConverter : IAstFromValueConverter
    {
        public IValue Convert(object value, IGraphType type)
        {
            return new FeatureValue((Feature)value);
        }

        public bool Matches(object value, IGraphType type)
        {
            return value is Feature;
        }
    }
}