#if TOOLS
using Godot;
using System;

[Tool]
[GlobalClass]
public partial class LogFilter : EditorPlugin
{
    private PanelContainer? _dockPanel;
    public override void _EnterTree()
    {
        // 1️⃣ 创建 Dock 面板
        _dockPanel = new PanelContainer();
        _dockPanel.Name = "Log Filter"; // Dock 标签名

        // 添加一个简单 Label 或按钮
        var vbox = new VBoxContainer();
        var label = new Label();
        label.Text = "这是一个自定义 Dock 面板！";
        vbox.AddChild(label);

        var button = new Button();
        button.Text = "点击测试";
        button.Pressed += () => GD.Print("按钮被点击了！");
        vbox.AddChild(button);

        _dockPanel.AddChild(vbox);

        // 2️⃣ 添加到Dock
        AddControlToDock(DockSlot.LeftUl ,_dockPanel);
    }

    public override void _ExitTree()
    {
        // 移除 Dock
        RemoveControlFromDocks(_dockPanel);
        _dockPanel?.Free();
    }
}
#endif
