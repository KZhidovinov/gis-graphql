namespace GisApi.ApiServer.GraphTypes.Scalars.ValueNodes
{
    using GraphQL.Language.AST;
    using NetTopologySuite.Features;

    public class FeatureValue : ValueNode<Feature>
    {
        public FeatureValue(Feature value)
        {
            this.Value = value;
        }

        protected override bool Equals(ValueNode<Feature> node)
        {
            return this.Value.Equals(node?.Value);
        }
    }
}