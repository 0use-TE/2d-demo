using System;
using System.Collections.Generic;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;

namespace GameDevTools.Services
{
    internal class NotificationService : INotificationService
    {
        private int _notificationTimeout = 10;
        private WindowNotificationManager? _notificationManager;

        public int NotificationTimeout
        {
            get => _notificationTimeout;
            set => _notificationTimeout = (value < 0) ? 0 : value;
        }

        /// <summary>设置宿主窗口。</summary>
        public void SetHostWindow(TopLevel hostWindow)
        {
            _notificationManager = new WindowNotificationManager(hostWindow)
            {
                Position = NotificationPosition.BottomRight,
                MaxItems = 4,
                Margin = new Thickness(0, 0, 15, 40)
            };
        }

        /// <summary>显示通知。</summary>
        public void Show(string title, string message, Action? onClick = null, NotificationType type = NotificationType.Information, int? timeoutSeconds = null)
        {
            if (_notificationManager is null)
                throw new InvalidOperationException("请先调用 SetHostWindow() 绑定宿主窗口。");

            _notificationManager.Show(new Notification(
                title,
                message,
                type,
                TimeSpan.FromSeconds(timeoutSeconds ?? _notificationTimeout),
                onClick));
        }

        // 信息通知
        public void ShowInfo(string title, string message, Action? onClick = null)
            => Show(title, message, onClick, NotificationType.Information);

        // 警告通知
        public void ShowWarning(string title, string message, Action? onClick = null)
            => Show(title, message, onClick, NotificationType.Warning);

        // 错误通知
        public void ShowError(string title, string message, Action? onClick = null)
            => Show(title, message, onClick, NotificationType.Error);

        // 成功通知
        public void ShowSuccess(string title, string message, Action? onClick = null)
            => Show(title, message, onClick, NotificationType.Success);
    }
}
