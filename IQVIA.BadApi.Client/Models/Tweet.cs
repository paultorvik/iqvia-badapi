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
        private string _jsonStamp;
        private DateTime _stamp = DateTime.MinValue;

        [DataMember(Name = "id")]
        public string ID { get; set; }

        [DataMember(Name = "stamp")]
        private string JsonStamp
        {
            get { return _jsonStamp; }
            set
            {
                _jsonStamp = value;
                if (!DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out _stamp))
                    _stamp = DateTime.MinValue;
            }
        }

        [IgnoreDataMember]
        public DateTime Stamp
        {
            get { return _stamp; }
            set
            {
                _stamp = value;
                _jsonStamp = Stamp.ToString("o");
            }
        }

        [DataMember(Name = "text")]
        public string Text { get; set; }
    }
}
