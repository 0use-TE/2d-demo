using Prism.Commands;
using Prism.Dialogs;
using ToolSets.Views;

namespace ToolSets.ViewModels
{
    internal class LogFilterViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;
        public  LogFilterViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }
        public DelegateCommand OpenConfigDialog => new DelegateCommand(async () =>
        {
            await _dialogService.ShowDialogAsync(nameof(LogFilterConfigView));
        });
    }
}
