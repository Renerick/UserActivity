using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UserActivity.CL.WPF.Entities;
using UserActivity.CL.WPF.Entities.RDF;
using UserActivity.CL.WPF.Entities.RDF.Mappers;

namespace UserActivity.CL.WPF.Services
{
    public class RDFUserActivityDataContext : BaseUserActivityDataContext
    {
        public const string RdfFileExtension = "rdf";

        private readonly IRDFMapper _mapper;

        public RDFUserActivityDataContext(IRDFMapper mapper)
        {
            _mapper = mapper;
        }

        public override void CloseSession(Guid sessionUID, DateTimeOffset endDateTime)
        {
            CurrentSession.EndDateTime = endDateTime;
            var result = new RDFRoot()
            {
                Session = new List<RDFSession>()
                {
                    _mapper.MapToRDF(CurrentSession)
                }
            };

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("us", RDFRoot.UsabilityNamespace);
            namespaces.Add("rdf", RDFRoot.RdfNamespace);

            var serializer = new XmlSerializer(typeof(RDFRoot));
            var fileName = CurrentSession.UID + "." + RdfFileExtension;
            using (var fileStream = File.Open(fileName, FileMode.OpenOrCreate))
            {
                serializer.Serialize(fileStream, result, namespaces);
            }

            CurrentSession = null;
            CurrentSessionGroup = null;
        }

        public static SessionGroup LoadSessionGroup(Stream xmlStream)
        {
            RDFRoot rdf = null;

            var serializer = new XmlSerializer(typeof(RDFRoot));
            using (xmlStream)
            {
                rdf = (RDFRoot)serializer.Deserialize(xmlStream);
            }

            return new SessionGroup
            {
                Sessions = { new RDFAutoMapper().MapFromRDF(rdf.Session[0]) }
            };
        }
    }
}