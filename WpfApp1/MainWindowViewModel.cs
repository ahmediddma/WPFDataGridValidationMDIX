using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp1
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<DataGridData> dataGridData;

        public ObservableCollection<DataGridData> DataGridData
        {
            get => dataGridData;
            set
            {
                dataGridData = value;
                NotifyOfPropertyChange();
            }
        }

        public MainWindowViewModel()
        {
            dataGridData = new ObservableCollection<DataGridData>
            {
                new DataGridData
                {
                    Id = 1,
                    Name = "Mouse",
                    Price = 59.99M
                },
                new DataGridData
                {
                    Id = 2,
                    Name = "Keyboard",
                    Price = 79.99M
                },
                new DataGridData
                {
                    Id = 3,
                    Name = "Ryzen",
                    Price = 0M
                },
                new DataGridData
                {
                    Id = 4,
                    Name = "",
                    Price = 799.99M
                }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyOfPropertyChange([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
