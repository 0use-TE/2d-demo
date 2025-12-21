using System;
using System.Collections.Generic;
using System.Text;

namespace GameDevTools.Share
{
    public static class SharedPathProvider
    {
        private const string BaseFolderName = "GameDevTools";
        private const string StorageFolderName = "Storage";

        /// <summary>
        /// 获取配置文件路径，并强制确保其父级目录物理存在
        /// </summary>
        public static string GetConfigFilePath(string key)
        {
            // 1. 拼接根路径
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var directory = Path.Combine(appData, BaseFolderName, StorageFolderName);

            // 2. 直接检查并创建（Directory.CreateDirectory 内部自带 Exists 检查，性能很高）
            // 这样做可以确保无论读写，目录一定存在
            Directory.CreateDirectory(directory);

            // 3. 返回完整文件路径
            var fileName = key.EndsWith(".json") ? key : $"{key}.json";
            return Path.Combine(directory, fileName);
        }
    }
}
