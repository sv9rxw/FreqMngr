using System.Windows;
using GalaSoft.MvvmLight.Ioc;
using MvvmDialogs;

using FreqMngr.ViewModels;
using FreqMngr.Views;

namespace FreqMngr
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            SimpleIoc.Default.Register<IDialogService>(() => new DialogService());
        }
    }
}
