using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace UserActivity.CL.WPF.Entities.RDF.Mappers
{
    public class EventsRDFCollectionResolver : IValueResolver<Variation, RDFVariation, EventsCollection>
    {
        public EventsCollection Resolve(Variation source, RDFVariation destination, EventsCollection destMember,
            ResolutionContext context)
        {
            var events = (IList<Event>)context.Items["events"];
            return new EventsCollection()
            {
                SingleClickEvents = context.Mapper.Map<IEnumerable<Event>,List<RDFEvent>>(events.Where(e => e.RegionName == source.RegionName && e.ImageName == source.Name && e.Kind == EventKind.Click)),
                CommandEvents = context.Mapper.Map<IEnumerable<Event>,List<RDFEvent>>(events.Where(e => e.RegionName == source.RegionName && e.ImageName == source.Name && e.Kind == EventKind.Command)),
            };
        }
    }

    public class EventListResolver : IValueResolver<RDFSession, Session, List<Event>>
    {
        public List<Event> Resolve(RDFSession source, Session destination, List<Event> destMember, ResolutionContext context)
        {
            var result = new List<Event>();

            foreach (var region in source.Contains.Regions)
            {
                foreach (var variation in region.Contains.Variations)
                {
                    foreach (var rdfEvent in variation.Contains.SingleClickEvents)
                    {
                        var newEvent = context.Mapper.Map<Event>(rdfEvent);
                        newEvent.RegionName = region.Name;
                        newEvent.ImageName = variation.Name;
                        newEvent.Kind = EventKind.Click;
                        result.Add(newEvent);
                    }

                    foreach (var rdfEvent in variation.Contains.CommandEvents)
                    {
                        var newEvent = context.Mapper.Map<Event>(rdfEvent);
                        newEvent.RegionName = region.Name;
                        newEvent.ImageName = variation.Name;
                        newEvent.Kind = EventKind.Command;
                        result.Add(newEvent);
                    }
                }
            }

            return result.OrderBy(e => e.DateTime).ToList();
        }
    }
}