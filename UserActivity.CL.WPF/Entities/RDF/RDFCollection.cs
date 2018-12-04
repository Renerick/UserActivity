using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace UserActivity.CL.WPF.Entities.RDF
{
    [Serializable]
    [XmlType(Namespace = RDFRoot.UsabilityNamespace)]
    public class BaseRDFCollection
    {
        [XmlAttribute("parseType", Namespace = RDFRoot.RdfNamespace)]
        public string RdfParseType { get; set; } = "Collection";
    }
}