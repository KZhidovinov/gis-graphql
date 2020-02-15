namespace GisApi.ApiServer.GraphTypes
{
    using System.Collections.Generic;

    public class TagsDictionary : Dictionary<string, string>
    {
        public TagsDictionary() : base()
        {
        }

        public TagsDictionary(IEnumerable<KeyValuePair<string, string>> pairs) : base(pairs)
        {
        }
    }
}
