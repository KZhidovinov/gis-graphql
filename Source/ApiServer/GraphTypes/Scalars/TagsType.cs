namespace GisApi.ApiServer.GraphTypes.Scalars
{
    using GisApi.ApiServer.GraphTypes.Scalars.ValueNodes;
    using GisApi.DataAccessLayer.Models;
    using GraphQL;
    using GraphQL.Language.AST;
    using GraphQL.Types;

    public class TagsType : ScalarGraphType
    {
        public TagsType() => this.Name = "Tags";

        public override object ParseLiteral(IValue value)
        {
            if (value is ObjectValue objectValue)
            {
                return this.ParseValue(objectValue.Value);
            }

            if (value is TagsValue tagsValue)
            {
                return this.ParseValue(tagsValue.Value);
            }

            return null;
        }

        public override object ParseValue(object value)
        {
            return ValueConverter.ConvertTo(value, typeof(TagsDictionary));
        }

        public override object Serialize(object value)
        {
            return ValueConverter.ConvertTo(value, typeof(TagsDictionary));
        }
    }
}