using System.Collections.Generic;
using System.Text;

namespace RabbitReplay.Record
{
    public static class DecodeHelpers
    {
        /// <summary>
        /// We usually want to decode a bunch of strings from dictionarys due to the rabbit API being a little basic,
        /// and if they're not what we expect err on the side of recording an empty string and keep going for now.
        /// </summary>
        public static string StringFromByteDictionaryOrDefault(IDictionary<string, object> dict, string key)
        {
            if (!dict.ContainsKey(key) || !(dict[key] is byte[] data)) return string.Empty;
            return Encoding.UTF8.GetString(data);
        }
    }
}
