using GisApi.ApiServer.GraphTypes.Scalars.ValueNodes;
using GraphQL.Language.AST;
using GraphQL.Types;

namespace GisApi.ApiServer.GraphTypes.Scalars.Converters
{
    public class TagsAstValueConverter : IAstFromValueConverter
    {
        public IValue Convert(object value, IGraphType type)
        {
            return new TagsValue((Tags)value);
        }

        public bool Matches(object value, IGraphType type)
        {
            return value is Tags;
        }
    }
}
