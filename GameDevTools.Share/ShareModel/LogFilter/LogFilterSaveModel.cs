using System;
using System.Collections.Generic;
using System.Text;

namespace GameDevTools.Share.ShareModel.LogFilter
{
    public class LogFilterSaveModel
    {
        public string SelectDllPath { get; set; } = string.Empty;
        public string SelectDllName { get; set; } = string.Empty;
        public List<NodeFilterDto> SavedNodes { get; set; } = new();
    }
}
