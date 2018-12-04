using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UserActivity.CL.WPF.Entities;

namespace UserActivity.CL.WPF.Services
{
    public abstract class BaseUserActivityDataContext : IUserActivityDataContext
    {
        public SessionGroup CurrentSessionGroup { get; protected set; }

        public Session CurrentSession { get; protected set; }

        public void OpenSession(Guid sessionUID, DateTimeOffset startDateTime)
        {
            CurrentSessionGroup = new SessionGroup();
            CurrentSession = new Session()
            {
                UID = sessionUID.ToString().ToUpper(),
                StartDateTime = startDateTime,
            };
            CurrentSessionGroup.Sessions.Add(CurrentSession);
        }

        public abstract void CloseSession(Guid sessionUID, DateTimeOffset endDateTime);

        public void WriteEvent(Guid sessionUID, Event activity)
        {
            var region = CurrentSession.Regions.FirstOrDefault(r => r.Name == activity.RegionName);
            if (region == null)
            {
                region = activity.Region;
                CurrentSession.Regions.Add(region);
            }
            else
            {
                var oldImage = region.Variations.FirstOrDefault(i => i.Name == activity.RegionName);
                var newImage = activity.Region.Variations.FirstOrDefault();
                if (newImage != null)
                {
                    if (oldImage == null)
                    {
                        region.Variations.Add(newImage);
                    }
                    else
                    {
                        region.Variations.Remove(oldImage);
                        region.Variations.Add(newImage);
                    }
                }
            }

            CurrentSession.Events.Add(activity);
        }

        public bool GetIsRegionImageExist(string regionName, string imageName)
        {
            bool isExist = false;

            var region = CurrentSession.Regions.FirstOrDefault(r => r.Name == regionName);
            if (region != null)
            {
                var image = region.Variations.FirstOrDefault(i => i.Name == imageName);
                isExist = image != null;
            }

            return isExist;
        }

    }
}