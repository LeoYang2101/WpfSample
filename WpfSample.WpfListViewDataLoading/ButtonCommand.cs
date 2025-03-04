using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfSample.WpfListViewDataLoading
{
    public class ButtonCommand : ICommand
    {
        private Action _whatToExecute;
        private Func<bool> _whenToExecute;

        /// <summary>
        /// 防止重入
        /// </summary>
        private bool _forbidReentrant = false;




        /// <summary>
        /// 运行标志位
        /// </summary>
        private bool _inExecute = false;

        private object _obj = new object();

        public ButtonCommand(Action what, Func<bool> when, bool forbidReentrant = true)
        {
            _whatToExecute = what;
            _whenToExecute = when;
            _forbidReentrant = forbidReentrant;
        }


        public bool CanExecute(object parameter)
        {
            if (_forbidReentrant && _inExecute) return false;

            return _whenToExecute();
        }

        public async void Execute(object parameter)
        {
            if (_forbidReentrant && _inExecute) return;
            _inExecute = true;
            _whatToExecute();
            _inExecute = false;
        }


        public event EventHandler CanExecuteChanged;
    }
}
