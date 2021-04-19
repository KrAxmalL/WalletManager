using DataStorage;
using Models.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Wallets
{
    public class DbWallet : IStorable
    {
        public string Name
        {
            get;
        }
        public string Description
        {
            get;
        }
        public Currencies.Currency Currency
        {
            get;
        }
        public decimal StartingBalance
        {
            get;
        }

        public decimal CurrentBalance
        {
            get;
        }

        public Guid OwnerGuid
        {
            get;
            set;
        }

        public Guid Guid
        {
            get;
            set;
        }

        public List<Guid> CategoriesGuids
        {
            get;
            set;
        }

        public List<Transaction> Transactions
        {
            get;
        }

        public DbWallet(string name, string description, Currencies.Currency currency, decimal startingBalance, Guid ownerGuid)
        {
            Guid = Guid.NewGuid();
            Name = name;
            Description = description;
            Currency = currency;
            StartingBalance = startingBalance;
            CurrentBalance = startingBalance;
            OwnerGuid = ownerGuid;
            Transactions = new List<Transaction>();
        }
    }
}

