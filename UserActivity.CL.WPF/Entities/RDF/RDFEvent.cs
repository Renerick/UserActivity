using System;
using System.Globalization;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace UserActivity.CL.WPF.Entities.RDF
{
    [Serializable]
    [XmlType(TypeName = "Event", Namespace = RDFRoot.UsabilityNamespace)]
    public class RDFEvent
    {
        public const string DateTimeFormat = "o";

        [XmlIgnore]
        public DateTimeOffset? DateTime { get; set; }

        [XmlAttribute("hasDateTime", Form = XmlSchemaForm.Qualified)]
        public string UtcDateTimeString
        {
            get => DateTime?.ToString(DateTimeFormat);
            set => DateTime = string.IsNullOrEmpty(value)
                ? null
                : (DateTimeOffset?) DateTimeOffset.ParseExact(value, DateTimeFormat, CultureInfo.CurrentCulture);
        }

        [XmlIgnore]
        public double? InRegionX { get; set; }

        [XmlAttribute("hasInRegionX", Form = XmlSchemaForm.Qualified)]
        public string InRegionXString
        {
            get => InRegionX?.ToString();
            set => InRegionX = string.IsNullOrEmpty(value) ? null : (double?) double.Parse(value);
        }

        [XmlIgnore]
        public double? InRegionY { get; set; }

        [XmlAttribute("hasInRegionY", Form = XmlSchemaForm.Qualified)]
        public string InRegionYString
        {
            get => InRegionY?.ToString();
            set => InRegionY = string.IsNullOrEmpty(value) ? null : (double?) double.Parse(value);
        }

        [XmlAttribute("hasName", Form = XmlSchemaForm.Qualified)]
        public string Name { get; set; }
    }

    [XmlType(TypeName = "SingleClickMouseEvent")]
    public class RDFSingleClickMouseEvent: RDFEvent
    {

    }

    [XmlType(TypeName = "CommandEvent")]
    public class RDFCommandEvent : RDFEvent
    {

    }
}