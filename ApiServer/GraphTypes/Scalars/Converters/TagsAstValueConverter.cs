namespace GisApi.ApiServer.GraphTypes.Scalars.Converters
{
    using GisApi.ApiServer.GraphTypes.Scalars.ValueNodes;
    using GraphQL.Language.AST;
    using GraphQL.Types;

    public class TagsAstValueConverter : IAstFromValueConverter
    {
        public IValue Convert(object value, IGraphType type)
        {
            return new TagsValue((TagsDictionary)value);
        }

        public bool Matches(object value, IGraphType type)
        {
            return value is TagsDictionary;
        }
    }
}
