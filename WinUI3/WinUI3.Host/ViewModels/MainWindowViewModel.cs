using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowTests.ViewModels
{
    public partial class  MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _title;

        public bool IsTargetProcessRunning { get; private set; }

        public MainWindowViewModel() 
        {
            Title = "This is MainWindow";
        }

        public void UpdateIsTargetProcessRunning()
        {
            IsTargetProcessRunning = Process.GetProcessesByName("CreatorZoneUI")?.Count() == 1;
            OnPropertyChanged(nameof(IsTargetProcessRunning));
        }
    }
}
