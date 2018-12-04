using AutoMapper;
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

        private static readonly IMapper RDFMapper;

        static RDFUserActivityDataContext()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Session, RDFSession>()
                    .ForMember(s => s.StartDateTimeString, opt => opt.Ignore())
                    .ForMember(s => s.EndDateTimeString, opt => opt.Ignore())
                    .ForPath(s => s.Contains.Regions, opt => opt.MapFrom(s => s.Regions));

                cfg.CreateMap<Region, RDFRegion>()
                    .ForPath(r => r.Contains.Variations, opt => opt.MapFrom(r => r.Variations));
                cfg.CreateMap<Variation, RDFVariation>()
                    .ForMember(v => v.Contains, opt => opt.ResolveUsing<EventsListResolver>());
                cfg.CreateMap<Event, RDFEvent>()
                    .ForMember(e => e.Name, opt => opt.MapFrom(e => e.CommandName));
            });


            mapperConfig.AssertConfigurationIsValid();

            RDFMapper = mapperConfig.CreateMapper();
        }

        public override void CloseSession(Guid sessionUID, DateTimeOffset endDateTime)
        {
            CurrentSession.EndDateTime = endDateTime;

            var events = CurrentSession.Events;

            var result = new RDFRoot()
            {
                Session = new List<RDFSession>()
                {
                    RDFMapper.Map<Session, RDFSession>(CurrentSession, opts => opts.Items.Add("events", events))
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
    }
}