using System.Linq;
using AutoMapper;

namespace UserActivity.CL.WPF.Entities.RDF.Mappers
{
    public class RDFAutoMapper : IRDFMapper
    {
        private static readonly IMapper AutoMapper;

        static RDFAutoMapper()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                CreateMappingsToRDF(cfg);
                CreateMappingsFromRDF(cfg);
            });

            AutoMapper = mapperConfig.CreateMapper();
        }

        public RDFSession MapToRDF(Session session)
        {
            var events = session.Events;
            return AutoMapper.Map<Session, RDFSession>(session, opts => opts.Items.Add("events", events));
        }

        public Session MapFromRDF(RDFSession session)
        {
            return AutoMapper.Map<RDFSession, Session>(session);
        }

        private static void CreateMappingsFromRDF(IProfileExpression cfg)
        {
            cfg.CreateMap<RDFSession, Session>()
               .ForMember(s => s.Regions, opt => opt.MapFrom(s => s.Contains.Regions))
               .ForMember(s => s.StartDateTimeString, opt => opt.Ignore())
               .ForMember(s => s.EndDateTimeString, opt => opt.Ignore())
               .ForMember(s => s.Events, opt => opt.MapFrom(s =>
                   s.Contains.Regions.SelectMany(r =>
                        r.Contains.Variations.SelectMany(v => v.Contains.SingleClickEvents)
                         .Concat(r.Contains.Variations.SelectMany(v => v.Contains.CommandEvents)))
                    .OrderBy(e => e.DateTime)));

            cfg.CreateMap<RDFRegion, Region>()
               .ForMember(r => r.Variations, opt => opt.MapFrom(r => r.Contains.Variations));

            cfg.CreateMap<RDFVariation, Variation>();

            cfg.CreateMap<RDFEvent, Event>()
               .ForMember(e => e.CommandName, opt => opt.MapFrom(e => e.Name));
        }

        private static void CreateMappingsToRDF(IProfileExpression cfg)
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
        }
    }
}