using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using WpfSample.WpfSqlite.Data;
using WpfSample.WpfSqlite.Models;

namespace WpfSample.WpfSqlite.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public RelayCommand SwitchItemCmd { get; }

        public MainViewModel() {
            SwitchItemCmd = new RelayCommand(() => { });
        }

    }

}
