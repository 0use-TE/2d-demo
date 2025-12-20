using System;
using System.Collections.Generic;
using System.Text;

namespace GameDevTools.Services.DataPersistences
{
    internal class JsonPersistenceService : IJsonPersistenceService
    {
        public T Load<T>(string key)
        {
            throw new NotImplementedException();
        }

        public void Save<T>(string key, T value)
        {
            throw new NotImplementedException();
        }
    }
}
