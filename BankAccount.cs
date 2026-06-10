namespace BankApp.Models
{
    public abstract class BankAccount
    {
        private readonly string _accountNumber;
        private string _ownerName;
        private decimal _balance;

        public string AccountNumber => _accountNumber;

        public string OwnerName
        {
            get => _ownerName;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Имя владельца не может быть пустым", nameof(value));
                }

                _ownerName = value.Trim();
            }
        }

        public decimal Balance => _balance;

        protected BankAccount(string accountNumber, string ownerName, decimal initialBalance)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                throw new ArgumentException("Номер счета не может быть пустым", nameof(accountNumber));
            }

            if (initialBalance < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(initialBalance), "Начальный баланс не может быть отрицательным");
            }

            _accountNumber = accountNumber.Trim();
            _ownerName = string.Empty;
            OwnerName = ownerName;
            _balance = initialBalance;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Сумма пополнения должна быть больше нуля");
            }

            _balance += amount;
        }

        public virtual void Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Сумма снятия должна быть больше нуля");
            }

            if (amount > _balance)
            {
                throw new InvalidOperationException("Недостаточно средств на счете");
            }

            _balance -= amount;
        }

        protected void ChangeBalance(decimal newBalance)
        {
            _balance = newBalance;
        }

        public abstract void DisplayInfo();
    }
}
