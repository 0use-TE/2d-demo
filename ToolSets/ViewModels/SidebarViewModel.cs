using Prism.Commands;
using Prism.Navigation.Regions;
using ToolSets.Views;

namespace ToolSets.ViewModels
{
    public class SidebarViewModel : ViewModelBase
    {
        private const int Collapsed = 40;
        private const int Expanded = 200;

        private readonly IRegionManager _regionManager;
        private int _flyoutWidth;

        public SidebarViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            Title = "Navigation";
            FlyoutWidth = Expanded;
        }

        public DelegateCommand LogFilterNavigation => new(() =>
        {
            _regionManager.RequestNavigate(RegionNames.ContentRegion, nameof(LogFilterView));
        });

        public DelegateCommand CmdFlyoutMenu => new(() =>
        {
            var isExpanded = FlyoutWidth == Expanded;
            FlyoutWidth = isExpanded ? Collapsed : Expanded;
        });

        public int FlyoutWidth { get => _flyoutWidth; set => SetProperty(ref _flyoutWidth, value); }
    }
}
