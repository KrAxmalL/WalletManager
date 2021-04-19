using DataStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Transactions
{
    public class DbTransaction : IStorable
    {

        public decimal AmountOfMoney
        {
            get;
        }

        public Currencies.Currency Currency
        {
            get;
        }

        public string Description
        {
            get;
        }

        public DateTimeOffset Date
        {
            get;
            set;
        }

        public Guid Guid
        {
            get;
            set;
        }

        public Guid WalletGuid
        {
            get;
            set;
        }

        public Guid OwnerGuid
        {
            get;
            set;
        }


        public DbTransaction(decimal amountOfMoney, Currencies.Currency currency, string description, DateTimeOffset date, Guid walletGuid, Guid ownerGuid)
        {
            Guid = Guid.NewGuid();
            AmountOfMoney = amountOfMoney;
            Currency = currency;
            Description = description;
            Date = date;
            WalletGuid = walletGuid;
            OwnerGuid = ownerGuid;
        }
    }
}

