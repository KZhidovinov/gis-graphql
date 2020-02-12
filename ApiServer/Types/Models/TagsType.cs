using GraphQL;
using GraphQL.Language.AST;
using GraphQL.Types;

namespace GisApi.ApiServer.Types.Models
{
    public class TagsType : ScalarGraphType
    {
        public TagsType() => Name = "Tags";

        public override object ParseLiteral(IValue value)
        {
            return value is StringValue stringValue
                ? ParseValue(stringValue.Value)
                : null;
        }

        public override object ParseValue(object value)
        {
            return ValueConverter.ConvertTo(value, typeof(Tags));
        }

        public override object Serialize(object value)
        {
            throw new System.NotImplementedException();
        }
    }
}