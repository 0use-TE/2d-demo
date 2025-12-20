using Avalonia.Styling;
using Prism.Commands;

namespace GameDevTools.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            SwitchThemeCommand = new DelegateCommand(() =>
            {
                if (App.Current == null)
                    return;
                if (App.Current.RequestedThemeVariant == ThemeVariant.Dark)
                    App.Current.RequestedThemeVariant = ThemeVariant.Light;
                else
                    App.Current.RequestedThemeVariant=ThemeVariant.Dark;
            });
        }
        public DelegateCommand SwitchThemeCommand { get; set; }

    }
}
