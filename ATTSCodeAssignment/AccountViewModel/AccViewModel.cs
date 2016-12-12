using AccountModel;
using AccountViewModel.Commands;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using System.Linq;
using AccountDataLayer;
using System.Collections.Generic;
using System.Windows;

namespace AccountViewModel
{
    public class AccViewModel : INotifyPropertyChanged
    {
        #region private members
        private string _FileName;
        private string _FullPathFileName;
        private decimal _ImportedLines;
        private decimal _TotalLinesToImport;
        private decimal _CurrentProgress;
        private List<string> _ImportMessage;
        private AccModel _Account;
        
        private readonly BackgroundWorker worker;
        #endregion

        #region Constructor
        public AccViewModel()
        {
            SelectFile = new SelectFileCommand(this);
            ImportFile = new ImportFileCommand(this);
            this.worker = new BackgroundWorker();
            this.worker.DoWork += this.SaveAccounts;
            this.worker.WorkerReportsProgress = true;
            this.worker.ProgressChanged += this.ProgressChanged; 
        }
        #endregion

        #region Properties
        
        /// <summary>
        /// Gets the Account instance.
        /// </summary>
        public AccModel Account
        {
            get { return _Account; }
        }

        /// <summary>
        /// Gets the current progress of the import.
        /// </summary>
        public decimal CurrentProgress
        {
            get { return this._CurrentProgress; }
            private set
            {
                if (this._CurrentProgress != value)
                {
                    this._CurrentProgress = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("CurrentProgress"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the file name to import.
        /// </summary>
        public string FileName
        {
            get { return _FileName; }
            private set 
            {
                this._FileName = value;
                PropertyChanged(this, new PropertyChangedEventArgs("FileName"));
            }
        }

        /// <summary>
        /// Gets or sets the number of imported lines.
        /// </summary>
        public decimal ImportedLines
        {
            get { return _ImportedLines; }
            set
            {
                _ImportedLines = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ImportedLines"));
            }
        }

        /// <summary>
        /// Gets or sets the total number of lines to save.
        /// </summary>
        public decimal TotalLinesToImport
        {
            get { return _TotalLinesToImport; }
            set
            {
                _TotalLinesToImport = value;
                PropertyChanged(this, new PropertyChangedEventArgs("TotalLinesToImport"));
            }
        }

        /// <summary>
        /// Gets or sets the the message after the import.
        /// </summary>
        public List<string> ImportMessage
        {
            get { return _ImportMessage; }
            set
            {
                _ImportMessage = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ImportMessage"));
            }
        }
        #endregion

        #region Commands
        /// <summary>
        /// Gets the SelectFile command for the view model.
        /// </summary>
        public ICommand SelectFile
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the ImportFile command for the view model.
        /// </summary>
        public ICommand ImportFile
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the command can be executed.
        /// </summary>
        public bool CanSelect
        {
            get { return true; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the command can be executed.
        /// The value is true if the file was selected and it is not empty.
        /// </summary>
        public bool CanImport
        {
            get { return !string.IsNullOrEmpty(FileName); }
        }
        #endregion

        /// <summary>
        /// Choose the file to be imported.
        /// </summary>
        public void ChooseFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".csv";
            dialog.Filter = "CSV Files (*.csv)|*.csv|XLSX Files (*.xlsx)|*.xlsx";

            if (dialog.ShowDialog() == true)
            {
                FileName = dialog.SafeFileName;
                _FullPathFileName = dialog.FileName;
            }
        }

        /// <summary>
        /// Import the file data.
        /// </summary>
        public void Import()
        {
            if (File.Exists(_FullPathFileName))
            {
                CurrentProgress = 0;
                TotalLinesToImport = 0;
                ImportedLines = 0;
                worker.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("The file does not exist anymore. Choose another one!");
                _FileName = null;
            }
            
        }

        /// <summary>
        /// Save the accounts to the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAccounts(object sender, DoWorkEventArgs e)
        {
            string[] tokens;
            int value;
            string line;
            List<string> failedLines = new List<string>();
            failedLines.Add("The following lines had errors: ");
            AccountDA da = new AccountDA();
            List<AccModel> listAccounts = new List<AccModel>();
            
            using (StreamReader sr = new StreamReader(_FullPathFileName))
            {
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    tokens = line.Split(',');

                    if (tokens.Length >= 4 && Int32.TryParse(tokens[3], out value) && ValidateCurrency(tokens[2]))
                    {
                        listAccounts.Add(new AccModel(tokens[0], tokens[1], tokens[2], value));
                        TotalLinesToImport++;
                    }
                    else
                    {
                        failedLines.Add(line);
                    }
                }
            }

            foreach (AccModel acc in listAccounts)
            {
                da.SaveAccounts(acc);
                ImportedLines++;
                worker.ReportProgress((int)(ImportedLines / (TotalLinesToImport == 0 ? 1 : TotalLinesToImport) * 100));

            }

            ImportMessage = failedLines;
            ImportMessage.Add("There were " + ImportedLines + " lines imported.");
        }

        /// <summary>
        /// Event handler for the progress change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.CurrentProgress = e.ProgressPercentage; 
        }

        /// <summary>
        /// Check the currency is a valid ISO 4217.
        /// </summary>
        /// <param name="currencyCode"></param>
        /// <returns></returns>
        private bool ValidateCurrency(string currencyCode)
        {
            int regionExists = (from culture in System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.InstalledWin32Cultures)
                                                          where culture.Name.Length > 0 && !culture.IsNeutralCulture
                                                          let region = new System.Globalization.RegionInfo(culture.LCID)
                                                          where String.Equals(region.ISOCurrencySymbol, currencyCode, StringComparison.InvariantCultureIgnoreCase)
                                                          select region).Count();

            return regionExists > 0 ? true : false;
        }
    }
}
