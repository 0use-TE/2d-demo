using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameDevTools.Services.DataPersistenceServices
{
    internal interface IViewModelDataInit
    {
        bool IsLoaded { get; }
        void Load();
        void Save();
    }
}
