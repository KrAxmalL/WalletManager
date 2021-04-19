using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Transactions
{
    public class Transaction
    {
        private readonly Guid _guid;
        private decimal _amountOFMoney;
        private Currencies.Currency _currency;
        private string _description;
        private DateTimeOffset _date;
        private Guid _walletGuid;
        private Guid _ownerGuid;

        public decimal AmountOfMoney
        {
            get { return _amountOFMoney; }
            set { _amountOFMoney = value; }
        }

        public Currencies.Currency Currency
        {
            get { return _currency; }
            set { _currency = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public DateTimeOffset Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public Guid Guid
        {
            get { return _guid; }
        }

        public Guid WalletGuid
        {
            get { return _walletGuid; }
        }

        public Guid OwnerGuid
        {
            get { return _ownerGuid; }
        }

        public Transaction(Guid guid, decimal amountOfMoney, Currencies.Currency currency, string description, DateTimeOffset date, Guid walletGuid, Guid ownerGuid)
        {
            this._guid = guid;
            this._amountOFMoney = amountOfMoney;
            this._currency = currency;
            this._description = description;
            this._date = date;
            this._walletGuid = walletGuid;
            this._ownerGuid = ownerGuid;
        }

        public override string ToString()
        {
            return $"Transaction: {_description}; on {_date}; amount: {_amountOFMoney}{Currencies.currencyToString(_currency)}";
        }

        public bool Validate()
        {
            if (_date == null)
            {
                return false;
            }

            return true;
        }
    }
}
