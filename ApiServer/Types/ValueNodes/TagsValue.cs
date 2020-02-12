﻿using GraphQL.Language.AST;

namespace GisApi.ApiServer.Types.ValueNodes
{
    public class TagsValue : ValueNode<Tags>
    {
        public TagsValue(Tags value)
        {
            Value = value;
        }

        protected override bool Equals(ValueNode<Tags> node)
        {
            return Value.Equals(node.Value);
        }
    }
}
