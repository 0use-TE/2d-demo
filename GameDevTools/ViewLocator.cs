using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using GameDevTools.ViewModels;

namespace GameDevTools;

internal class ViewLocator : IDataTemplate
{
    // 静态缓存：ViewModelType -> ViewType
    private static readonly Dictionary<Type, Type> Cache  = new();
    private static readonly Lock Lock = new();

    public Control? Build(object? data)
    {
        if (data is null)
            return null;

        var viewModelType = data.GetType();

        if (!TryGetViewType(viewModelType, out var viewType))
        {
            return new TextBlock
            {
                Text = $"Not Found: {viewModelType.FullName}"
            };
        }

        return (Control)Activator.CreateInstance(viewType)!;
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }

    private static bool TryGetViewType(Type viewModelType, out Type viewType)
    {
        lock (Lock)
        {
            // 1️⃣ 命中缓存
            if (Cache.TryGetValue(viewModelType, out viewType!))
                return true;

            // 2️⃣ 计算 View 类型名
            var viewTypeName = viewModelType.FullName!.Replace("ViewModel", "View");

            viewType = Type.GetType(viewTypeName)!;

            // 3️⃣ 找不到直接返回 false（不缓存）
            if (viewType is null)
                return false;

            // 4️⃣ 写入缓存
            Cache[viewModelType] = viewType;
            return true;
        }
    }
}
