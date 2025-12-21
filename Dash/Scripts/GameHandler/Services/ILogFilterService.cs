using System;
using System.Collections.Generic;
using System.Text;

namespace Dash.Scripts.GameHandler.Services
{
    public interface ILogFilterService
    {
        bool IsAllowed(string categoryName);
    }
}
