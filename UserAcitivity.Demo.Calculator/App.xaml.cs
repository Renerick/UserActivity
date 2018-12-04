using System.Windows;
using UserActivity.CL.WPF.Entities.RDF.Mappers;
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
            UserActivityService.Initialize(new RDFUserActivityDataContext(new RDFMapper()));
            UserActivityService.Current.OpenSession();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            UserActivityService.Current.CloseSession();
            base.OnExit(e);
        }
    }
}
