using System.Windows;
using UserActivity.CL.WPF.Services;

namespace UserAcitivity.Demo.Calculator
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            UserActivityService.Initialize(new RDFUserActivityDataContext());
            UserActivityService.Current.OpenSession();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            UserActivityService.Current.CloseSession();
            base.OnExit(e);
        }
    }
}
