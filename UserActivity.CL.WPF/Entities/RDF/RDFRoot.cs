using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace UserActivity.CL.WPF.Entities.RDF
{
    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = RDFRoot.RdfNamespace, ElementName = "RDF")]
    public class RDFRoot
    {
        [XmlIgnore] public const string UsabilityNamespace = "https://w3id.org/usability#";

        [XmlIgnore] public const string RdfNamespace = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";

        [XmlElement(Namespace = UsabilityNamespace)]
        public List<RDFSession> Session { get; set; }
    }
}