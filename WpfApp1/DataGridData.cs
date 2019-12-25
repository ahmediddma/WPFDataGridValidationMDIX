using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WpfApp1
{
    public class DataGridData : INotifyPropertyChanged, INotifyDataErrorInfo, IEditableObject
    {
        public int Id
        {
            get => id;
            set
            {
                id = value;
                NotifyOfPropertyChange();
            }
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                NotifyOfPropertyChange();

                _errors = new Dictionary<string, List<string>>();
                if (!_errors.TryGetValue(nameof(Name), out List<string> errorsForName))
                {
                    errorsForName = new List<string>();
                }
                else
                {
                    errorsForName.Clear();
                }
                if (string.IsNullOrWhiteSpace(name))
                {
                    errorsForName.Add("Field Required");
                }
                _errors[nameof(Name)] = errorsForName;
                GetErrors(nameof(Name));
                RaiseErrorsChanged();
            }
        }

        public decimal Price
        {
            get => price;
            set
            {
                price = value;
                NotifyOfPropertyChange();

                _errors = new Dictionary<string, List<string>>();
                if (!_errors.TryGetValue(nameof(Price), out List<string> errorsForPrice))
                {
                    errorsForPrice = new List<string>();
                }
                else
                {
                    errorsForPrice.Clear();
                }
                if (price <= 0)
                {
                    errorsForPrice.Add("Enter a value more than zero");
                }
                _errors[nameof(Price)] = errorsForPrice;
                GetErrors(nameof(Price));
                RaiseErrorsChanged();
            }
        }

        public bool HasErrors
        {
            get => _errors.Any(x=>x.Value.Count > 0);
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return string.IsNullOrEmpty(propertyName)
                ? Array.Empty<object>()
                : _errors.TryGetValue(propertyName, out List<string> errors) ? errors : (IEnumerable)Array.Empty<object>();
        }

        private DataGridData backupCopy;
        private bool inEdit;

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public void BeginEdit()
        {
            if (inEdit) return;
            inEdit = true;
            backupCopy = this.MemberwiseClone() as DataGridData;
        }

        public void CancelEdit()
        {
            if (!inEdit) return;
            inEdit = false;
            if (backupCopy != null)
            {
                this.Id = backupCopy.Id;
                this.Name = backupCopy.Name;
                this.Price = backupCopy.Price;
            }
        }

        public void EndEdit()
        {
            if (!inEdit) return;
            inEdit = false;
            backupCopy = null;
        }

        public void RaiseErrorsChanged([CallerMemberName] String propertyName = "")
        {
            EventHandler<DataErrorsChangedEventArgs> handler = ErrorsChanged;
            if (handler == null) return;
            var arg = new DataErrorsChangedEventArgs(propertyName);
            handler.Invoke(this, arg);
        }

        private void NotifyOfPropertyChange([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();
        private int id;
        private string name;
        private decimal price;
    }
}
