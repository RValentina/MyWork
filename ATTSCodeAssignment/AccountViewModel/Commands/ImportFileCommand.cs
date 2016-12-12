using System;
using System.Windows.Input;

namespace AccountViewModel.Commands
{
    internal class ImportFileCommand : ICommand
    {
        private AccViewModel _ViewModel;

        #region Constructor
        public ImportFileCommand(AccViewModel viewModel)
        {
            _ViewModel = viewModel;
        }
        #endregion

        #region ICommand members

        /// <summary>
        /// Gets or sets a value indicating whether the command can be executed.
        /// </summary>
        public bool CanExecute(object parameter)
        {
            return _ViewModel.CanImport;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _ViewModel.Import();
        }
        #endregion
    }
}
