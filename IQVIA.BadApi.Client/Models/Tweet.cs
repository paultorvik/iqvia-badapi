using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace IQVIA.BadApi.Client.Models
{
    /// <summary>
    /// Tweet data structure that can be downloaded from REST API in JSON format
    /// </summary>
    [DataContract]
    public class Tweet
    {
        [DataMember(Name = "id")]
        public string ID { get; set; }

        [DataMember(Name = "stamp")]
        private string JsonStamp { get; set; }

        [IgnoreDataMember]
        public DateTime Stamp { get; set; }

        [DataMember(Name = "text")]
        public string Text { get; set; }

        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            Stamp = DateTime.Parse(JsonStamp, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        }

        [OnSerializing]
        void OnSerializing(StreamingContext context)
        {
            JsonStamp = Stamp.ToString("o");
        }
    }
}
