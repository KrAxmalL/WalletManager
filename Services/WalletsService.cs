using DataStorage;
using Models;
using Models.Wallets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    public class WalletsService
    {

        private static FileDataStorage<DbWallet> _storage = new FileDataStorage<DbWallet>();

        public static async Task<List<Wallet>> GetWalletsAsync()
        {
            var walletsInDatabase = await _storage.GetAllAsync();
            var resultingWallets = new List<Wallet>();
            foreach (var wallet in walletsInDatabase)
            {
                Trace.WriteLine("Wallet owner: " + wallet.OwnerGuid);
                Trace.WriteLine("Curr user: " + AuthentificationService.ActiveUser.Guid);
                if (wallet.OwnerGuid == AuthentificationService.ActiveUser.Guid)
                {
                    Wallet walletToAdd = new Wallet(wallet.Guid, wallet.Name, wallet.Description, wallet.Currency, wallet.StartingBalance, wallet.CurrentBalance, wallet.OwnerGuid);
                    resultingWallets.Add(walletToAdd);
                }
            }
            return resultingWallets;
        }

        public static async Task<bool> AddWallet(Wallet walletToAdd)
        {
            var wallets = await _storage.GetAllAsync();
            var wallet = wallets.FirstOrDefault(wallet => ((wallet.Name == walletToAdd.Name) && (AuthentificationService.ActiveUser.Guid == wallet.OwnerGuid)));
            if (wallet != null)
            {
                throw new Exception("Such wallet already exists! Name must be unique!");
            }

            if (String.IsNullOrWhiteSpace(walletToAdd.Name))
            {
                throw new Exception("Wallet name is empty!");
            }

            DbWallet dbWalletToAdd = new DbWallet(walletToAdd.Name, walletToAdd.Description, walletToAdd.Currency, walletToAdd.CurrentBalance, walletToAdd.OwnerGuid);
            await _storage.AddOrUpdateAsync(dbWalletToAdd);
            return true;
        }

        public static async Task<bool> DeleteWallet(Wallet walletToDelete)
        {
            var wallets = await _storage.GetAllAsync();
            var wallet = wallets.FirstOrDefault(wallet => ((wallet.Name == walletToDelete.Name) && (wallet.OwnerGuid == walletToDelete.OwnerGuid)));
            if (wallet == null)
            {
                throw new Exception("Such wallet doesn't exist!");
            }
            _storage.Remove(wallet.Guid);
            return true;
        }

        public static async Task<bool> EditWallet(Wallet walletToEdit, string newName, string newDescription, Currencies.Currency newCurrency)
        {
            if (newName == null)
            {
                throw new Exception("Name of wallet can't be blank!");
            }
            else
            {
                var wallets = await _storage.GetAllAsync();
                var wallet = wallets.FirstOrDefault(wallet => ((wallet.Name == newName) && (wallet.OwnerGuid == walletToEdit.OwnerGuid)));
                if (wallet != null)
                {
                    throw new Exception("Such wallet already exists! Name must be unique!");
                }
                else
                {
                    Trace.WriteLine("WalletToEdit guid:" + walletToEdit.Guid);
                    _storage.Remove(walletToEdit.Guid);
                    walletToEdit.Name = newName;
                    walletToEdit.Description = newDescription;
                    walletToEdit.Currency = newCurrency;
                    DbWallet dbWalletToEdit = new DbWallet(walletToEdit.Name, walletToEdit.Description, walletToEdit.Currency, walletToEdit.StartingBalance, walletToEdit.OwnerGuid);
                    await _storage.AddOrUpdateAsync(dbWalletToEdit);
                    return true;
                }
            }
        }

        public static List<Wallet> GetWallets()
        {
            var walletsInDatabase = _storage.GetAll();
            var resultingWallets = new List<Wallet>();
            foreach (var wallet in walletsInDatabase)
            {
                Trace.WriteLine("Wallet owner: " + wallet.OwnerGuid);
                Trace.WriteLine("Curr user: " + AuthentificationService.ActiveUser.Guid);
                if (wallet.OwnerGuid == AuthentificationService.ActiveUser.Guid)
                {
                    Wallet walletToGet = new Wallet(wallet.Guid, wallet.Name, wallet.Description, wallet.Currency, wallet.StartingBalance, wallet.CurrentBalance, wallet.OwnerGuid);
                    resultingWallets.Add(walletToGet);
                }
            }
            return resultingWallets;
        }
    }
}
