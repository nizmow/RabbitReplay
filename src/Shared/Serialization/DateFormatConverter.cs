using Newtonsoft.Json.Converters;

namespace RabbitReplay.Shared.Serialization
{
    public class DateFormatConverter : IsoDateTimeConverter
    {
        public DateFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }
}
