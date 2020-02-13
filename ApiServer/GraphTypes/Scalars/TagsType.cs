using GisApi.ApiServer.GraphTypes.Scalars.ValueNodes;
using GraphQL;
using GraphQL.Language.AST;
using GraphQL.Types;

namespace GisApi.ApiServer.GraphTypes.Scalars
{
    public class TagsType : ScalarGraphType
    {
        public TagsType() => Name = "Tags";

        public override object ParseLiteral(IValue value)
        {
            if (value is ObjectValue objectValue)
                return ParseValue(objectValue.Value);

            if (value is TagsValue tagsValue)
                return ParseValue(tagsValue.Value);

            return null;
        }

        public override object ParseValue(object value)
        {
            return ValueConverter.ConvertTo(value, typeof(Tags));
        }

        public override object Serialize(object value)
        {
            return ValueConverter.ConvertTo(value, typeof(Tags));
        }
    }
}