using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

namespace UserActivity.CL.WPF.Entities
{
    [Serializable]
    [XmlType(AnonymousType = true)]
    public class Session
    {
        public const string DateTimeFormat = "yyyy.MM.dd HH:mm:ss";

        [XmlAttribute]
        public string UID { get; set; }

        [XmlIgnore]
        public DateTimeOffset? StartDateTime { get; set; }

        [XmlAttribute("UtcStartDateTime")]
        public string StartDateTimeString
        {
            get => StartDateTime?.ToString(DateTimeFormat);
            set => StartDateTime = string.IsNullOrEmpty(value) ? null : (DateTimeOffset?)DateTimeOffset.ParseExact(value, DateTimeFormat, CultureInfo.CurrentCulture);
        }

        [XmlIgnore]
        public DateTimeOffset? EndDateTime { get; set; }

        [XmlAttribute("UtcEndDateTime")]
        public string EndDateTimeString
        {
            get => EndDateTime?.ToString(DateTimeFormat);
            set => EndDateTime = string.IsNullOrEmpty(value) ? null : (DateTimeOffset?)DateTimeOffset.ParseExact(value, DateTimeFormat, CultureInfo.CurrentCulture);
        }

        [XmlArrayItem("Region", IsNullable = true)]
        public List<Region> Regions { get; private set; } = new List<Region>();

        [XmlArrayItem("Event", IsNullable = true)]
        public List<Event> Events { get; private set; } = new List<Event>();
    }
}
