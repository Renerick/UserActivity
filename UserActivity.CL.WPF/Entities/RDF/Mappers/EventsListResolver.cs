using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace UserActivity.CL.WPF.Entities.RDF.Mappers
{
    public class EventsListResolver : IValueResolver<Variation, RDFVariation, EventsCollection>
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
}