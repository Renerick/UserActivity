using System;
using System.Collections.Generic;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace UserActivity.CL.WPF.Entities.RDF
{
    [Serializable]
    [XmlType(AnonymousType = true)]
    public class RDFRegion
    {
        [XmlAttribute("hasName", Namespace = RDFRoot.UsabilityNamespace, Form = XmlSchemaForm.Qualified)]
        public string Name { get; set; }

        [XmlElement("contains", IsNullable = true)]
        public VariationsCollection Contains { get; set; }
    }

    [Serializable]
    public class VariationsCollection : BaseRDFCollection
    {
        [XmlElement("Variation", Namespace = RDFRoot.UsabilityNamespace)]
        public List<RDFVariation> Variations { get; set; }
    }
}
