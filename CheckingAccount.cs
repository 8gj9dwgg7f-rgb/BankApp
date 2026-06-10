namespace BankApp.Models
{
    public class CheckingAccount : BankAccount
    {
        public CheckingAccount(string accountNumber, string ownerName, decimal initialBalance)
            : base(accountNumber, ownerName, initialBalance)
        {
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Обычный счет {AccountNumber}: владелец - {OwnerName}, баланс - {Balance:C}");
        }
    }
}
