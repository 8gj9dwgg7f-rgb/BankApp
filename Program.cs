using System.Globalization;
using BankApp.Models;
using BankApp.Services;

var bank = new Bank();
bool running = true;

while (running)
{
    ClearConsole();
    Console.WriteLine("=== BankApp ===");
    Console.WriteLine("1. Создать счет");
    Console.WriteLine("2. Пополнить счет");
    Console.WriteLine("3. Снять деньги");
    Console.WriteLine("4. Показать информацию о счете");
    Console.WriteLine("5. Показать все счета");
    Console.WriteLine("0. Выход");
    Console.Write("Выбор: ");

    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            CreateAccount(bank);
            break;
        case "2":
            Deposit(bank);
            break;
        case "3":
            Withdraw(bank);
            break;
        case "4":
            ShowAccount(bank);
            break;
        case "5":
            ShowAllAccounts(bank);
            break;
        case "0":
            running = false;
            break;
        default:
            Console.WriteLine("Неверный выбор.");
            Pause();
            break;
    }
}

static void CreateAccount(Bank bank)
{
    try
    {
        Console.WriteLine("Тип счета:");
        Console.WriteLine("1. Обычный");
        Console.WriteLine("2. Сберегательный");
        Console.WriteLine("3. Кредитный");
        Console.Write("Выбор: ");
        string accountType = ReadRequiredLine();

        Console.Write("Владелец: ");
        string ownerName = ReadRequiredLine();

        Console.Write("Начальный баланс: ");
        decimal initialBalance = ReadDecimal();

        string accountNumber = bank.GenerateAccountNumber();
        BankAccount account = accountType switch
        {
            "1" => new CheckingAccount(accountNumber, ownerName, initialBalance),
            "2" => new SavingsAccount(accountNumber, ownerName, initialBalance, ReadSavingsRate()),
            "3" => new CreditAccount(accountNumber, ownerName, initialBalance, ReadCreditLimit()),
            _ => throw new ArgumentException("Неизвестный тип счета")
        };

        bank.AddAccount(account);
        Console.WriteLine($"Счет создан. Номер счета: {account.AccountNumber}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка: {GetFriendlyErrorMessage(ex)}");
    }

    Pause();
}

static decimal ReadSavingsRate()
{
    Console.Write("Процентная ставка: ");
    return ReadDecimal();
}

static decimal ReadCreditLimit()
{
    Console.Write("Кредитный лимит: ");
    return ReadDecimal();
}

static void Deposit(Bank bank)
{
    try
    {
        string accountNumber = ReadAccountNumber();
        Console.Write("Сумма пополнения: ");
        decimal amount = ReadDecimal();

        bank.Deposit(accountNumber, amount);
        Console.WriteLine("Счет пополнен.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка: {GetFriendlyErrorMessage(ex)}");
    }

    Pause();
}

static void Withdraw(Bank bank)
{
    try
    {
        string accountNumber = ReadAccountNumber();
        Console.Write("Сумма снятия: ");
        decimal amount = ReadDecimal();

        bank.Withdraw(accountNumber, amount);
        Console.WriteLine("Операция выполнена.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка: {GetFriendlyErrorMessage(ex)}");
    }

    Pause();
}

static void ShowAccount(Bank bank)
{
    string accountNumber = ReadAccountNumber();
    BankAccount? account = bank.FindAccountByNumber(accountNumber);

    if (account is null)
    {
        Console.WriteLine("Счет с таким номером не найден.");
    }
    else
    {
        account.DisplayInfo();
    }

    Pause();
}

static void ShowAllAccounts(Bank bank)
{
    List<BankAccount> accounts = bank.GetAllAccounts();

    if (accounts.Count == 0)
    {
        Console.WriteLine("Список счетов пуст.");
    }
    else
    {
        foreach (BankAccount account in accounts)
        {
            account.DisplayInfo();
        }
    }

    Pause();
}

static string ReadAccountNumber()
{
    Console.Write("Номер счета: ");
    return ReadRequiredLine();
}

static string ReadRequiredLine()
{
    string? value = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(value))
    {
        throw new ArgumentException("Значение не может быть пустым");
    }

    return value.Trim();
}

static decimal ReadDecimal()
{
    string value = ReadRequiredLine();

    if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal currentCultureNumber))
    {
        return currentCultureNumber;
    }

    string normalizedValue = value.Replace(',', '.');

    if (decimal.TryParse(normalizedValue, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal invariantNumber))
    {
        return invariantNumber;
    }

    throw new FormatException("Введите число");
}

static string GetFriendlyErrorMessage(Exception ex)
{
    if (ex is ArgumentException argumentException && !string.IsNullOrWhiteSpace(argumentException.ParamName))
    {
        return ex.Message
            .Replace($" (Parameter '{argumentException.ParamName}')", string.Empty)
            .Trim();
    }

    return ex.Message;
}

static void Pause()
{
    Console.WriteLine("Нажмите любую клавишу...");

    if (!Console.IsInputRedirected)
    {
        Console.ReadKey(true);
    }
}

static void ClearConsole()
{
    if (Console.IsInputRedirected || Console.IsOutputRedirected)
    {
        return;
    }

    Console.Clear();
}
