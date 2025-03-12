using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WpfSample.WpfAutofac
{
    public class MainViewModel
    {
        private readonly IMyService _myService;


        //public MainViewModel(IMyService myService,ILifetimeScope lifetimeScope)
        public MainViewModel(ILifetimeScope lifetimeScope)
        {
            //_myService = myService;
            _myService = lifetimeScope.Resolve<IMyService>();

            DoSomethingCommand = new RelayCommand(DoSomething, CanExecuteDoSomethingCommand);
        }

        public ICommand DoSomethingCommand { get; }

        public void DoSomething()
        {
            _myService.DoSomething();            
        }
        private bool CanExecuteDoSomethingCommand()
        {
            return true;
        }
    }
}
