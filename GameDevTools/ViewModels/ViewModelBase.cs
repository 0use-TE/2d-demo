using Prism.Mvvm;

namespace GameDevTools.ViewModels
{
    public class ViewModelBase : BindableBase
    {
        private string _title = string.Empty;

        /// <summary>Gets or sets the title of the view.</summary>
        public string Title { get => _title; set => SetProperty(ref _title, value); }
    }
}
