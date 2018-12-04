using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace UserActivity.CL.WPF.Entities.RDF
{
    [XmlType(TypeName = "Variation", Namespace = RDFRoot.UsabilityNamespace)]
    public class RDFVariation
    {
        [XmlAttribute("hasWidth", Namespace = RDFRoot.UsabilityNamespace, Form = XmlSchemaForm.Qualified)]
        public double Width { get; set; }

        [XmlAttribute("hasHeight", Namespace = RDFRoot.UsabilityNamespace, Form = XmlSchemaForm.Qualified)]
        public double Height { get; set; }

        [XmlAttribute("hasName", Namespace = RDFRoot.UsabilityNamespace, Form = XmlSchemaForm.Qualified)]
        public string Name { get; set; }

        [XmlIgnore]
        public byte[] Data { get; set; }

        [XmlElement("hasImage", Namespace = RDFRoot.UsabilityNamespace)]
        public XmlCDataSection DataSection
        {
            get
            {
                var dataString = Data == null ? string.Empty : Convert.ToBase64String(Data);
                return new XmlDocument().CreateCDataSection(dataString);
            }
            set
            {
                var dataString = value.Value;
                Data = Convert.FromBase64String(dataString);
            }
        }

        [XmlElement("contains")]
        public EventsCollection Contains { get; set; }
    }

    [Serializable]
    public class EventsCollection : BaseRDFCollection
    {
        [XmlElement("SingleClickMouseEvent", Namespace = RDFRoot.UsabilityNamespace)]
        public List<RDFEvent> SingleClickEvents { get; set; }

        [XmlElement("CommandEvent", Namespace = RDFRoot.UsabilityNamespace)]
        public List<RDFEvent> CommandEvents { get; set; }
    }
}