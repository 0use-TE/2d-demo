using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using GameDevTools.Extensions;
using GameDevTools.Misc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.FileIO;

namespace GameDevTools.Services.DataPersistenceServices
{
    internal class JsonPersistenceService : IJsonPersistenceService
    {
        private readonly ILogger<JsonPersistenceService> _logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };
        private string GetApplicationDataFolder() => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public JsonPersistenceService(ILogger<JsonPersistenceService> logger)
        {
            _logger = logger;
        }

        public T Load<T>(string key) where T : new()
        {
            try
            {
                var directory = Path.Combine(GetApplicationDataFolder(), Settings.JsonStorageBasePathName);
                Directory.CreateDirectory(directory); // 确保目录存在

                var loadPath = Path.Combine(directory, key);

                // 确保扩展名是 .json
                if (!Path.HasExtension(loadPath) || Path.GetExtension(loadPath).ToLower() != ".json")
                    loadPath += ".json";

                // 如果文件不存在，直接创建默认对象并保存
                if (!File.Exists(loadPath))
                {
                    var defaultObj = new T();
                    var defaultJson = JsonSerializer.Serialize(defaultObj, _jsonSerializerOptions);
                    File.WriteAllText(loadPath, defaultJson);
                    _logger.LogInformationWithArea(E_LogArea.FileIO, "文件不存在，新建文件保存成功!: {key}", key);
                    return defaultObj;
                }

                var json = File.ReadAllText(loadPath);
                var obj = JsonSerializer.Deserialize<T>(json);
                _logger.LogInformationWithArea(E_LogArea.FileIO, "加载 JSON 文件成功: {key}", key);
                // 如果反序列化失败，返回默认对象
                return obj ?? new T();
            }
            catch (Exception ex)
            {
                _logger.LogErrorWithArea(E_LogArea.FileIO, ex, "加载 JSON 文件失败: {key}", key);
                return new T();
            }
        }

        public void Save<T>(string key, T value) where T : new()
        {
            try
            {
                // 1. 构建目录路径
                var directory = Path.Combine(GetApplicationDataFolder(), Settings.JsonStorageBasePathName);
                Directory.CreateDirectory(directory); // 确保目录存在

                // 2. 构建文件路径
                var savePath = Path.Combine(directory, key);

                // 3. 确保扩展名是 .json
                if (!Path.HasExtension(savePath) || Path.GetExtension(savePath).ToLower() != ".json")
                    savePath += ".json";

                // 4. 如果传入对象为空，则创建默认对象
                if (value == null)
                {
                    value = new T();
                }

                // 5. 序列化为 JSON
                var json = JsonSerializer.Serialize(value, _jsonSerializerOptions);

                File.WriteAllText(savePath, json);

                _logger.LogInformationWithArea(E_LogArea.FileIO, "保存 JSON 文件: {key}", key);
            }
            catch (Exception ex)
            {
                _logger.LogErrorWithArea(E_LogArea.FileIO, ex, "保存 JSON 文件失败: {key}", key);
            }
        }

    }
}
