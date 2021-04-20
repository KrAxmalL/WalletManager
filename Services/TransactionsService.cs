using DataStorage;
using Models;
using Models.Transactions;
using Models.Wallets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TransactionsService
    {
        private static FileDataStorage<DbTransaction> _storage = new FileDataStorage<DbTransaction>();

        public static async Task<List<Transaction>> GetTransactionsAsync()
        {
            var transactionsInDatabase = await _storage.GetAllAsync();
            var resultingTransactions = new List<Transaction>();
            foreach (var transaction in transactionsInDatabase)
            {
                Trace.WriteLine("Transaction owner: " + transaction.WalletGuid);
                Trace.WriteLine("Curr user: " + AuthentificationService.ActiveUser.Guid);
                if (transaction.OwnerGuid == AuthentificationService.ActiveUser.Guid)
                {
                    Transaction transactionToAdd = new Transaction(transaction.Guid, transaction.AmountOfMoney, transaction.Currency, transaction.Description,
                                                                  transaction.Date, transaction.WalletGuid, transaction.OwnerGuid);
                    resultingTransactions.Add(transactionToAdd);
                }
            }
            return resultingTransactions;
        }

        public static List<Transaction> GetTransactions()
        {
            var transactionsInDatabase = _storage.GetAll();
            Trace.WriteLine("Transactions in database: " + transactionsInDatabase.Count);
            var resultingTransactions = new List<Transaction>();
            foreach (var transaction in transactionsInDatabase)
            {
                Trace.WriteLine("Transaction owner: " + transaction.OwnerGuid);
                Trace.WriteLine("Transaction wallet: " + transaction.WalletGuid);
                Trace.WriteLine("Curr user: " + AuthentificationService.ActiveUser.Guid);
                if (transaction.OwnerGuid == AuthentificationService.ActiveUser.Guid)
                {
                    Transaction transactionToAdd = new Transaction(transaction.Guid, transaction.AmountOfMoney, transaction.Currency, transaction.Description,
                                                                  transaction.Date, transaction.WalletGuid, transaction.OwnerGuid);
                    resultingTransactions.Add(transactionToAdd);
                }
            }
            Trace.WriteLine("Transactions in resulting list: " + resultingTransactions.Count);
            return resultingTransactions;
        }

        public static async Task<bool> AddTransaction(Transaction transactionToAdd)
        {
            DbTransaction dbTransactionToAdd = new DbTransaction(transactionToAdd.AmountOfMoney, transactionToAdd.Currency, transactionToAdd.Description,
                                                                  transactionToAdd.Date, transactionToAdd.WalletGuid, transactionToAdd.OwnerGuid);
            await _storage.AddOrUpdateAsync(dbTransactionToAdd);
            return true;
        }

        public static async Task<bool> DeleteTransaction(Transaction transactionToDelete)
        {
            var transactions = await _storage.GetAllAsync();
            var transaction = transactions.FirstOrDefault(trans => trans.Guid == transactionToDelete.Guid);
            if (transaction == null)
            {
                throw new Exception("Such transaction doesn't exist!");
            }
            _storage.Remove(transactionToDelete.Guid);
            return true;
        }

        public static async Task<bool> EditTransaction(Transaction transactionToEdit, Wallet wallet, decimal newAmount, DateTimeOffset newDate, string newDescription, Currencies.Currency newCurrency)
        {
            var transactions = await _storage.GetAllAsync();
            Trace.WriteLine("TransactionToEdit guid:" + transactionToEdit.Guid);
            _storage.Remove(transactionToEdit.Guid);
            try
            {
                wallet.RemoveTransaction(transactionToEdit);
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
            transactionToEdit.AmountOfMoney = newAmount;
            transactionToEdit.Date = newDate;
            transactionToEdit.Description = newDescription;
            transactionToEdit.Currency = newCurrency;           
            wallet.AddTransaction(transactionToEdit);

            DbTransaction dbTransactionToEdit = new DbTransaction(transactionToEdit.AmountOfMoney, transactionToEdit.Currency, transactionToEdit.Description,
                                                                  transactionToEdit.Date, transactionToEdit.WalletGuid, transactionToEdit.OwnerGuid);
            await _storage.AddOrUpdateAsync(dbTransactionToEdit);
            return true;
        }
    }
}
