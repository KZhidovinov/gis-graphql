namespace GisApi.ApiServer.GraphTypes.Scalars.ValueNodes
{
    using GraphQL.Language.AST;
    using NetTopologySuite.Geometries;

    public class GeometryValue : ValueNode<Point>
    {
        public GeometryValue(Point value)
        {
            this.Value = value;
        }

        protected override bool Equals(ValueNode<Point> node)
        {
            return this.Value.Equals(node?.Value);
        }
    }
}