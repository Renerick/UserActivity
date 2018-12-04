using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UserActivity.CL.WPF.Entities;
using UserActivity.CL.WPF.Entities.RDF;

namespace UserActivity.CL.WPF.Services
{
    public class RDFUserActivityDataContext : BaseUserActivityDataContext
    {
        public const string RdfFileExtension = "rdf";

        public override void CloseSession(Guid sessionUID, DateTimeOffset endDateTime)
        {
            CurrentSession.EndDateTime = endDateTime;

            var events = CurrentSession.Events;

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Session, RDFSession>()
                    .ForMember(s => s.StartDateTimeString, opt => opt.Ignore())
                    .ForMember(s => s.EndDateTimeString, opt => opt.Ignore())
                    .ForPath(s => s.Contains.Regions, opt => opt.MapFrom(s => s.Regions))
                    .ForMember(s => s.UID, opt => opt.AddTransform(s => s.Replace("-", "")));

                cfg.CreateMap<Region, RDFRegion>()
                    .ForPath(r => r.Contains.Variations, opt => opt.MapFrom(r => r.Variations));
                cfg.CreateMap<Variation, RDFVariation>()
                    .ForPath(v => v.Contains.SingleClickEvents, opt => opt.MapFrom(v => events.Where(e => e.RegionName == v.RegionName && e.ImageName == v.Name && e.Kind == EventKind.Click)))
                    .ForPath(v => v.Contains.CommandEvents,opt => opt.MapFrom(v => events.Where(e => e.RegionName == v.RegionName && e.ImageName == v.Name && e.Kind == EventKind.Command)));
                cfg.CreateMap<Event, RDFEvent>()
                    .ForMember(e => e.Name, opt => opt.MapFrom(e => e.CommandName));
            });


            mapperConfig.AssertConfigurationIsValid();

            var rdfMapper = mapperConfig.CreateMapper();

            var result = new RDFRoot()
            {
                Session = new List<RDFSession>()
                    {
                        rdfMapper.Map<Session, RDFSession>(CurrentSession)
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