using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolSets.Services;

namespace ToolSets.ViewModels
{
    internal class TestViewModel:ViewModelBase
    {
        private readonly INotificationService notificationService;
        public List<string> strings = ["Ouse", "Touken", "123"];
        public TestViewModel(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }
        public DelegateCommand TestCommand => new DelegateCommand(() =>
        {
            notificationService.Show("测试", "Ouse&&Touken");
        });
    }
}
