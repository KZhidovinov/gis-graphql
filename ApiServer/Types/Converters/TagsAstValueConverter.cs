using GraphQL.Language.AST;
using GraphQL.Types;
using System;

namespace GisApi.ApiServer.Types.Converters
{
    public class TagsAstValueConverter : IAstFromValueConverter
    {
        public IValue Convert(object value, IGraphType type)
        {
            throw new NotImplementedException();
        }

        public bool Matches(object value, IGraphType type)
        {
            throw new NotImplementedException();
        }
    }
}
