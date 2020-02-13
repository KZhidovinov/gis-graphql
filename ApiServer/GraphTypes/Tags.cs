using System.Collections.Generic;

namespace GisApi.ApiServer.GraphTypes
{
    public class Tags : Dictionary<string, string>
    {
        public Tags() : base()
        {
        }

        public Tags(IEnumerable<KeyValuePair<string, string>> pairs) : base(pairs)
        {
        }
    }
}
