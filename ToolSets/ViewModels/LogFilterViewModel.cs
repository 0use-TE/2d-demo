using Prism.Commands;
using Prism.Dialogs;
using Prism.Ioc;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using ToolSets.Shared;
using ToolSets.Views;

namespace ToolSets.ViewModels
{
    public class LogFilterViewModel : BindableBase
    {
        private readonly ILogFilterService _logFilterService;
        private readonly IDialogService _dialogService;
        private ObservableCollection<LogFilterRule> _filterRules;

        public LogFilterViewModel(IContainerProvider container, IDialogService dialogService)
        {
            _logFilterService = container.Resolve<ILogFilterService>();
            _dialogService = dialogService;
            _filterRules = new ObservableCollection<LogFilterRule>(_logFilterService.GetFilterRules());

            ScanCommand = new DelegateCommand(ExecuteScan);
            EnableAllCommand = new DelegateCommand(ExecuteEnableAll);
            DisableAllCommand = new DelegateCommand(ExecuteDisableAll);
            OpenConfigDialogCommand = new DelegateCommand(ExecuteOpenConfigDialog);
            SaveCommand = new DelegateCommand(ExecuteSave);
        }

        public ObservableCollection<LogFilterRule> FilterRules
        {
            get => _filterRules;
            set => SetProperty(ref _filterRules, value);
        }

        public ICommand ScanCommand { get; }
        public ICommand EnableAllCommand { get; }
        public ICommand DisableAllCommand { get; }
        public ICommand OpenConfigDialogCommand { get; }
        public ICommand SaveCommand { get; }

        private void ExecuteScan()
        {
            var dllPath = _logFilterService.GetDllPath();
            if (string.IsNullOrEmpty(dllPath))
            {
                _dialogService.ShowDialog("LogFilterConfigView", null, _ => { });
                dllPath = _logFilterService.GetDllPath();
            }

            if (!string.IsNullOrEmpty(dllPath))
            {
                var rules = _logFilterService.ScanAssembly(dllPath);
                FilterRules.Clear();
                foreach (var rule in rules)
                {
                    FilterRules.Add(rule);
                }
            }
        }

        private void ExecuteEnableAll()
        {
            Debug.WriteLine("全部启用");
            foreach (var rule in FilterRules)
            {
                rule.IsEnabled = true;
            }
            _logFilterService.SaveFilterRules(FilterRules.ToList());
        }

        private void ExecuteDisableAll()
        {
            Debug.WriteLine("全部禁用");
            foreach (var rule in FilterRules)
            {
                rule.IsEnabled = false;
            }
            _logFilterService.SaveFilterRules(FilterRules.ToList());
        }

        private void ExecuteOpenConfigDialog()
        {
            _dialogService.ShowDialog("LogFilterConfigView", null, _ => { });
        }

        private void ExecuteSave()
        {
            _logFilterService.SaveFilterRules(FilterRules.ToList());
        }
    }
}
