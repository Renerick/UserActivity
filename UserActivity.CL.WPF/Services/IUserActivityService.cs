using System;
using UserActivity.CL.WPF.Entities;

namespace UserActivity.CL.WPF.Services
{
    public interface IUserActivityService
    {
        Guid? CurrentSessionUID { get; }
        DateTimeOffset? CurrentSessionStartDateTime { get; }

        void OpenSession();
        void CloseAndOpenSession();
        void CloseSession();

        void RegisterEvent(EventInfo evInfo);
    }
}
