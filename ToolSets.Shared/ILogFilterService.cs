using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolSets.Shared
{
    public interface ILogFilterService
    {
        List<LogFilterRule> ScanAssembly(string assemblyPath);
        List<LogFilterRule> GetFilterRules();
        void SaveFilterRules(List<LogFilterRule> rules);
        void UpdateFilterRules(List<LogFilterRule> newRules);
        string? GetDllPath();
        void SaveDllPath(string path);
    }
}
