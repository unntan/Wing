using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Wing.Other
{
    class ViewModelBase : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            var h = PropertyChanged;
            if (h == null) return;
            h(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var h = PropertyChanged;
            if (h == null) return;
            h(this, new PropertyChangedEventArgs(propertyName));
        }

        #region IDataErrorInfo ----------------------------------------

        private Dictionary<string, string> _ErrorMessages = new Dictionary<string, string>();

        string IDataErrorInfo.Error
        {
            get { return (_ErrorMessages.Count > 0) ? "Has Error" : null; }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (_ErrorMessages.ContainsKey(columnName))
                    return _ErrorMessages[columnName];
                else
                    return null;
            }
        }

        protected void SetError(string errorMessage, [CallerMemberName] string propertyName = "")
        {
            _ErrorMessages[propertyName] = errorMessage;
        }

        protected void ClearError([CallerMemberName] string propertyName = "")
        {
            if (_ErrorMessages.ContainsKey(propertyName))
            {
                _ErrorMessages.Remove(propertyName);
            }
        }

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName)) return null;
            if (!_ErrorMessages.ContainsKey(propertyName)) return null;
            return _ErrorMessages[propertyName];
        }

        public bool HasError
        {
            get { return _ErrorMessages.Count != 0; }
        }

        #endregion IDataErrorInfo ----------------------------------------

    }
}
