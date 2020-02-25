using System.Collections.Generic;
using System.Linq;
using NetTopologySuite.Features;

namespace GisApi.ApiServer
{
    public static class AttributesExtensions
    {
        public static Dictionary<string, object> ToDictionary(this IAttributesTable attrs) => new Dictionary<string, object>(
                attrs?.GetNames()
                    .Aggregate(new List<KeyValuePair<string, object>>().AsEnumerable(),
                        (acc, name) => acc.Append(KeyValuePair.Create(name, attrs[name]))
                    ));
    }
}
