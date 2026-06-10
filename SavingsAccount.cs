namespace BankApp.Models
{
    public class SavingsAccount : BankAccount
    {
        private decimal _interestRate;

        public decimal InterestRate
        {
            get => _interestRate;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Процентная ставка не может быть отрицательной");
                }

                _interestRate = value;
            }
        }

        public SavingsAccount(string accountNumber, string ownerName, decimal initialBalance, decimal interestRate)
            : base(accountNumber, ownerName, initialBalance)
        {
            InterestRate = interestRate;
        }

        public decimal GetProjectedBalance()
        {
            return Balance + Balance * InterestRate / 100;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine(
                $"Сберегательный счет {AccountNumber}: владелец - {OwnerName}, баланс - {Balance:C}, ставка - {InterestRate}%, расчетный баланс - {GetProjectedBalance():C}");
        }
    }
}
