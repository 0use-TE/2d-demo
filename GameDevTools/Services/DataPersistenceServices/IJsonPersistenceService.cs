using System;
using System.Collections.Generic;
using System.Text;

namespace GameDevTools.Services.DataPersistenceServices
{
    internal interface IJsonPersistenceService
    {
        T Load<T>(string key) where T : new();
        void Save<T>(string  key, T value) where T : new();
    }
}
