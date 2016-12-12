using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AccountViewModel;

namespace AccountViewModel
{
    internal class SelectFileCommand : ICommand
    {
        public SelectFileCommand(AccountViewModel viewModel)
        {
            _ViewModel = viewModel;
        }

        private AccViewModel _ViewModel;

        #region ICommand Members

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _ViewModel.CanSelect;
        }

        public void Execute(object parameter)
        {
            _ViewModel.ChooseFile();
        }
        #endregion
    }
}
