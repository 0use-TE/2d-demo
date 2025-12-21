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
    public Control? Build(object? data)
    {
        if (data == null) return null;

        // 1. 获取对应的 View 类型名称
        var viewTypeName = data.GetType().FullName?.Replace("ViewModel", "View");

        if (string.IsNullOrEmpty(viewTypeName)) return new TextBlock { Text = "无效的类型名称" };

        var viewType = Type.GetType(viewTypeName);
        if (viewType != null)
        {
            // 2. 实例化 View
            var view = (Control)Activator.CreateInstance(viewType)!;

            // 3. 关键：设置 DataContext
            view.DataContext = data;

            return view;
        }

        return new TextBlock { Text = $"找不到视图: {viewTypeName}" };
    }

    public bool Match(object? data)
    {
        // 只要是继承自 ViewModelBase 的数据对象，都由这个 Locator 处理
        return data is ViewModelBase;
    }
}
