using System;
using UserActivity.CL.WPF.Entities;

namespace UserActivity.CL.WPF.Services
{
    public class UserActivityService : IUserActivityService
    {
        private static readonly object SyncObj = new object();
        private static IUserActivityService _current;

        static UserActivityService()
        {
            var defaultDataContext = new DebugUserActivityDataContext();
            Initialize(defaultDataContext);
        }

        public static IUserActivityService Current
        {
            get { lock (SyncObj) return _current; }
            set { lock (SyncObj) _current = value; }
        }

        public static void Initialize(IUserActivityDataContext dataContext)
        {
            var service = new UserActivityService(dataContext);
            Current = service;
        }

        public UserActivityService(IUserActivityDataContext dataContext)
        {
            CurrentDataContext = dataContext ?? throw new ArgumentNullException();
        }

        public Guid? CurrentSessionUID { get; protected set; }

        public DateTimeOffset? CurrentSessionStartDateTime { get; protected set; }

        public IUserActivityDataContext CurrentDataContext { get; protected set; }

        public void OpenSession()
        {
            if (CurrentSessionUID.HasValue)
            {
                CloseSession();
            }
            CurrentSessionUID = Guid.NewGuid();
            CurrentSessionStartDateTime = DateTimeOffset.Now;
            CurrentDataContext.OpenSession(CurrentSessionUID.Value, CurrentSessionStartDateTime.Value);
        }

        public void CloseAndOpenSession()
        {
            if (CurrentSessionUID.HasValue)
            {
                CloseSession();
            }
            OpenSession();
        }

        public void CloseSession()
        {
            if (!CurrentSessionUID.HasValue) return;

            CurrentDataContext.CloseSession(CurrentSessionUID.Value, DateTimeOffset.Now);
            CurrentSessionUID = null;
            CurrentSessionStartDateTime = null;
        }

        public void RegisterEvent(EventInfo evInfo)
        {
            if (!CurrentSessionUID.HasValue) return;

            var ev = new Event()
            {
                DateTime = DateTimeOffset.Now,
                Kind = evInfo.Kind,
                InRegionX = evInfo.InRegionX,
                InRegionY = evInfo.InRegionY,
                RegionName = evInfo.RegionName,
                ImageName = $"{evInfo.RegionWidth}x{evInfo.RegionHeight}",
                RegionWidth = evInfo.RegionWidth,
                RegionHeight = evInfo.RegionHeight,
                Region = new Region() { Name = evInfo.RegionName },
                CommandName = evInfo.CommandName,
            };

            if (!CurrentDataContext.GetIsRegionImageExist(ev.RegionName, ev.ImageName))
            {
                var regionImage = evInfo.CreateRegionImage();
                regionImage.Name = ev.ImageName;
                regionImage.RegionName = evInfo.RegionName;
                ev.Region.Variations.Add(regionImage);
            }

            CurrentDataContext.WriteEvent(CurrentSessionUID.Value, ev);
        }
    }
}
