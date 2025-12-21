using System;
using System.Collections.Generic;
using System.Text;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;

namespace GameDevTools.Services
{
    internal interface INotificationService
    {
        int NotificationTimeout { get; set; }
        void SetHostWindow(TopLevel window);
        void Show(string title, string message, Action? onClick = null, NotificationType type = NotificationType.Information, int? timeoutSeconds = null);
        void ShowInfo(string title, string message, Action? onClick = null);
        void ShowWarning(string title, string message, Action? onClick = null);
        void ShowError(string title, string message, Action? onClick = null);
        void ShowSuccess(string title, string message, Action? onClick = null);
    }
}
