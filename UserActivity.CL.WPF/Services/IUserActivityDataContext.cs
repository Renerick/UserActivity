using System;
using UserActivity.CL.WPF.Entities;

namespace UserActivity.CL.WPF.Services
{
    public interface IUserActivityDataContext
    {
        void OpenSession(Guid sessionUID, DateTimeOffset start);
        void CloseSession(Guid sessionUID, DateTimeOffset end);
        void WriteEvent(Guid sessionUID, Event ev);
        bool GetIsRegionImageExist(string regionName, string imageName);
    }
}
