using System;
using System.Windows.Input;

namespace AccountViewModel.Commands
{
    internal class SelectFileCommand : ICommand
    {

        private AccViewModel _ViewModel;

        #region Constructor
        public SelectFileCommand(AccViewModel viewModel)
        {
            _ViewModel = viewModel;
        }
        #endregion

        #region ICommand Members

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the command can be executed.
        /// </summary>
        public bool CanExecute(object parameter)
        {
            return _ViewModel.CanSelect;
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _ViewModel.ChooseFile();
        }
        #endregion
    }
}
