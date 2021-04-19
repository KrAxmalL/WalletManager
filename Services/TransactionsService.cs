using DataStorage;
using Models;
using Models.Transactions;
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
            //var allTransactions = await _storage.GetAllAsync();
            DbTransaction dbTransactionToAdd = new DbTransaction(transactionToAdd.AmountOfMoney, transactionToAdd.Currency, transactionToAdd.Description,
                                                                  transactionToAdd.Date, transactionToAdd.WalletGuid, transactionToAdd.OwnerGuid);
            await _storage.AddOrUpdateAsync(dbTransactionToAdd);
            return true;
        }

        public static async Task<bool> DeleteTransaction(Transaction transactionToDelete)
        {
            var transactions = await _storage.GetAllAsync();
            _storage.Remove(transactionToDelete.Guid);
            return true;
        }

        //change arguments list to edit transactions
        public static async Task<bool> EditTransaction(Transaction transactionToEdit, string newName, string newDescription, Currencies.Currency newCurrency)
        {
            if (newName == null)
            {
                throw new Exception("Name of transaction can't be blank!");
            }
            else
            {
                var transactions = await _storage.GetAllAsync();
                Trace.WriteLine("TransactionToEdit guid:" + transactionToEdit.Guid);
                _storage.Remove(transactionToEdit.Guid);
                transactionToEdit.Description = newDescription;
                transactionToEdit.Currency = newCurrency;

                /*DbTransaction dbTransactionToEdit = new DbTransaction(transaction.AmountOfMoney, transaction.Currency, transaction.Description,
                                                                      transaction.Date, transaction.Category, transaction.WalletGuid, transaction.OwnerGuid);
                await _storage.AddOrUpdateAsync(dbTransactionToEdit);*/
                return true;
            }
        }
    }
}
