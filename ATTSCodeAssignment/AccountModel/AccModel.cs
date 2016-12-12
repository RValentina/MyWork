using System.ComponentModel;

namespace AccountModel
{
    public class AccModel : INotifyPropertyChanged
    {
        #region private members
        private string _Account;
        private string _Description;
        private string _CurrencyCode;
        private int _Value;
        #endregion

        #region Constructor
        public AccModel(string account, string description, string currencyCode, int value)
        {
            _Account = account;
            _Description = description;
            _CurrencyCode = currencyCode;
            _Value = value;
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties
        public string Account
        {
            get { return _Account; }
            set 
            { 
                _Account = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Account"));
            }
        }

        public string Description
        {
            get { return _Description; }
            set 
            { 
                _Description = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Description"));
            }
        }

        public string CurrencyCode
        {
            get { return _CurrencyCode; }
            set 
            { 
                _CurrencyCode = value;
                PropertyChanged(this, new PropertyChangedEventArgs("CurrencyCode"));
            }
        }

        public int Value
        {
            get { return _Value; }
            set 
            { 
                _Value = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Value"));
            }
        }

        #endregion
    }
}
