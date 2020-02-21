using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GisApi.ApiServer.Test
{
    public static class EqualityExtensions
    {
        public static bool EqualsTo(this Dictionary<string, object> me, Dictionary<string, object> other)
        {
            return me.All(p => other.ContainsKey(p.Key)
                && (other[p.Key].Equals(p.Value)
                    || (p.Value as Dictionary<string, object>).EqualsTo(
                        other[p.Key] as Dictionary<string, object>)
                    )
            );
        }

    }
}
