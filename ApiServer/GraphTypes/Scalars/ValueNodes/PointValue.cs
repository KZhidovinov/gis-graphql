using GraphQL.Language.AST;
using NetTopologySuite.Geometries;

namespace GisApi.ApiServer.GraphTypes.Scalars.ValueNodes
{
    public class PointValue : ValueNode<Point>
    {
        public PointValue(Point value)
        {
            Value = value;
        }

        protected override bool Equals(ValueNode<Point> node)
        {
            return Value.Equals(node.Value);
        }
    }
}
