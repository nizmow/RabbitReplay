using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RabbitReplay.Record
{
    public static class DecodeHelpers
    {
        public static string FromBase64DictionaryKeyOrDefault(IDictionary<string, object> dict, string key)
        {
            if (!dict.ContainsKey(key) || !(dict[key] is byte[] data)) return string.Empty;
            return FromBase64BytesToDecodedString(data);
        }

        public static string FromBase64BytesToDecodedString(byte[] encodedData)
        {
            var encodedString = Encoding.ASCII.GetString(encodedData);
            return encodedString;
            //return Encoding.UTF8.GetString(Convert.FromBase64String(encodedString));
        }

        public static Dictionary<string, string> ConvertBase64Dictionary(IDictionary<string, object> dict)
        {
            return dict.ToDictionary(
                d => d.Key,
                d => FromBase64BytesToDecodedString((byte[])d.Value));
        }
    }
}
