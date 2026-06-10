using System.Collections.Generic;
using System.Linq;
using BankApp.Models;

namespace BankApp.Services
{
    public class Bank
    {
        private readonly List<BankAccount> _accounts = new();
        private int _nextAccountNumber = 1001;

        public string GenerateAccountNumber()
        {
            return $"ACC-{_nextAccountNumber++}";
        }

        public void AddAccount(BankAccount account)
        {
            if (FindAccountByNumber(account.AccountNumber) is not null)
            {
                throw new InvalidOperationException("Счет с таким номером уже существует");
            }

            _accounts.Add(account);
        }

        public List<BankAccount> GetAllAccounts()
        {
            return new List<BankAccount>(_accounts);
        }

        public BankAccount? FindAccountByNumber(string accountNumber)
        {
            return _accounts.FirstOrDefault(account =>
                account.AccountNumber.Equals(accountNumber, StringComparison.OrdinalIgnoreCase));
        }

        public List<SavingsAccount> GetSavingsAccounts()
        {
            return _accounts.OfType<SavingsAccount>().ToList();
        }

        public void Deposit(string accountNumber, decimal amount)
        {
            BankAccount account = GetRequiredAccount(accountNumber);
            account.Deposit(amount);
        }

        public void Withdraw(string accountNumber, decimal amount)
        {
            BankAccount account = GetRequiredAccount(accountNumber);
            account.Withdraw(amount);
        }

        private BankAccount GetRequiredAccount(string accountNumber)
        {
            return FindAccountByNumber(accountNumber)
                ?? throw new InvalidOperationException("Счет с таким номером не найден");
        }
    }
}
