﻿using System;
using System.Globalization;
using System.Xml.Serialization;

namespace UserActivity.CL.WPF.Entities
{
    [Serializable]
    [XmlType(AnonymousType = true)]
    public partial class Event
    {
        public const string DateTimeFormat = "o";

        [XmlIgnore]
        public DateTimeOffset? DateTime { get; set; }

        [XmlIgnore]
        public DateTimeOffset? LocalDateTime => DateTime?.ToLocalTime();

        [XmlAttribute("UtcDateTime")]
        public string UtcDateTimeString
        {
            get => DateTime?.ToString(DateTimeFormat);
            set => DateTime = string.IsNullOrEmpty(value) ? null : (DateTimeOffset?)DateTimeOffset.ParseExact(value, DateTimeFormat, CultureInfo.CurrentCulture);
        }

        [XmlAttribute]
        public EventKind Kind { get; set; }

        [XmlIgnore]
        public double? InRegionX { get; set; }

        [XmlAttribute("InRegionX")]
        public string InRegionXString
        {
            get => InRegionX?.ToString();
            set => InRegionX = string.IsNullOrEmpty(value) ? null : (double?)double.Parse(value);
        }

        [XmlIgnore]
        public double? InRegionY { get; set; }

        [XmlAttribute("InRegionY")]
        public string InRegionYString
        {
            get => InRegionY?.ToString();
            set => InRegionY = string.IsNullOrEmpty(value) ? null : (double?)double.Parse(value);
        }

        [XmlAttribute]
        public string RegionName { get; set; }

        [XmlAttribute]
        public string ImageName { get; set; }

        [XmlAttribute]
        public string CommandName { get; set; }

        [XmlIgnore]
        public double RegionWidth { get; set; }

        [XmlIgnore]
        public double RegionHeight { get; set; }

        [XmlIgnore]
        public Region Region { get; set; }
    }
}
