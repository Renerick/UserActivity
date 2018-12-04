using System;
using System.IO;
using System.Xml.Serialization;
using UserActivity.CL.WPF.Entities;

namespace UserActivity.CL.WPF.Services
{
    public class XmlUserActivityDataContext : BaseUserActivityDataContext
    {
        public const string UadFileExtension = "uad";

        public override void CloseSession(Guid sessionUID, DateTimeOffset endDateTime)
        {
            CurrentSession.EndDateTime = endDateTime;

            var serializer = new XmlSerializer(typeof(SessionGroup));
            var fileName = CurrentSession.UID + "." + UadFileExtension;
            using (var fileStream = File.Open(fileName, FileMode.OpenOrCreate))
            {
                serializer.Serialize(fileStream, CurrentSessionGroup);
            }

            CurrentSession = null;
            CurrentSessionGroup = null;
        }

        public static SessionGroup LoadSessionGroup(Stream xmlStream)
        {
            SessionGroup sessionGroup = null;

            var serializer = new XmlSerializer(typeof(SessionGroup));
            using (xmlStream)
            {
                sessionGroup = (SessionGroup)serializer.Deserialize(xmlStream);
            }

            return sessionGroup;
        }
    }
}
