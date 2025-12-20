using System;
using System.Collections.Generic;
using System.Text;

namespace GameDevTools.Services.DataPersistences
{
    internal interface IJsonPersistenceService
    {
        T Load<T>(string key);
        void Save<T>(string  key, T value);
    }
}
