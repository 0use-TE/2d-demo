    using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ToolSets.Shared
{
    public class LogFilterRule : INotifyPropertyChanged
    {
        private string _typeName = string.Empty;
        private string _fieldOrPropertyName = string.Empty;
        private bool _isEnabled = true;
        private string _logLevel = "Information";

        public event PropertyChangedEventHandler? PropertyChanged;

        [JsonPropertyName("typeName")]
        public string TypeName
        {
            get => _typeName;
            set
            {
                if (_typeName != value)
                {
                    _typeName = value;
                    OnPropertyChanged();
                }
            }
        }

        [JsonPropertyName("fieldOrPropertyName")]
        public string FieldOrPropertyName
        {
            get => _fieldOrPropertyName;
            set
            {
                if (_fieldOrPropertyName != value)
                {
                    _fieldOrPropertyName = value;
                    OnPropertyChanged();
                }
            }
        }

        [JsonPropertyName("isEnabled")]
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        [JsonPropertyName("logLevel")]
        public string LogLevel
        {
            get => _logLevel;
            set
            {
                if (_logLevel != value)
                {
                    _logLevel = value;
                    OnPropertyChanged();
                }
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
