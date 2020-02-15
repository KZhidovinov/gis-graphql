namespace GisApi.ApiServer.GraphTypes.Scalars.ValueNodes
{
    using GraphQL.Language.AST;
    using NetTopologySuite.Geometries;

    public class GeometryValue : ValueNode<Point>
    {
        public GeometryValue(Point value)
        {
            Value = value;
        }

        protected override bool Equals(ValueNode<Point> node)
        {
            return Value.Equals(node.Value);
        }
    }
}
