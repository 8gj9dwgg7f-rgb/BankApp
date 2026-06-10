namespace BankApp.Models
{
    public class CreditAccount : BankAccount
    {
        private decimal _creditLimit;

        public decimal CreditLimit
        {
            get => _creditLimit;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Кредитный лимит не может быть отрицательным");
                }

                _creditLimit = value;
            }
        }

        public CreditAccount(string accountNumber, string ownerName, decimal initialBalance, decimal creditLimit)
            : base(accountNumber, ownerName, initialBalance)
        {
            CreditLimit = creditLimit;
        }

        public override void Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Сумма снятия должна быть больше нуля");
            }

            if (Balance - amount < -CreditLimit)
            {
                throw new InvalidOperationException("Превышен кредитный лимит");
            }

            ChangeBalance(Balance - amount);
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Кредитный счет {AccountNumber}: владелец - {OwnerName}, баланс - {Balance:C}, лимит - {CreditLimit:C}");
        }
    }
}
