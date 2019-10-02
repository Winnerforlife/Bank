using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    public class Bank
    {
        private static ulong clientsCounter;

        private readonly Dictionary<ulong, Client> clients = new Dictionary<ulong, Client>();

        public List<Client> GetClients()
        {
            return clients.Values.ToList();
        }

        public Client GetClient(ulong id)
        {
            Client client;
            if (!clients.TryGetValue(id, out client))
            {
                throw new InvalidOperationException();
            }

            return client;
        }

        public void AddClient(Client client)
        {
            client.ID = ++clientsCounter;
            clients[client.ID] = client;
        }

        public void EditClient(Client client)
        {
            clients[client.ID] = client;
        }

        public void DeleteClient(ulong id)
        {
            if (!clients.ContainsKey(id))
            {
                throw new InvalidOperationException();
            }

            clients.Remove(id);
        }
    }
    public class Client
    {
        private static ulong accountsCounter;

        private readonly Dictionary<ulong, Account> accounts = new Dictionary<ulong, Account>();

        public ulong ID { get; set; }

        public string FullName { get; set; }

        public uint Age { get; set; }

        public string WorkPlace { get; set; }

        public List<Account> GetAccounts()
        {
            return accounts.Values.ToList();
        }

        public Account GetAccount(ulong id)
        {
            Account account;
            if (!accounts.TryGetValue(id, out account))
            {
                throw new InvalidOperationException();
            }

            return account;
        }

        public void CreateAccount(Account account)
        {
            account.ID = ++accountsCounter;
            accounts[account.ID] = account;
        }
    }
    //public enum ActionType
    //{
    //    Loan,
    //    Deposit
    //}

    public class AccountAction
    {
        public DateTime Date { get; set; }

        //public ActionType Type { get; set; }

        public decimal Amount { get; set; }
    }
    public class Account
    {
        //private readonly List<AccountAction> history = new List<AccountAction>();

        public ulong ID { get; set; }

        public bool IsOpen { get; set; }

        public decimal Balance { get; set; }

        //public List<AccountAction> GetHistory()
        //{
        //    return history;
        //}

        //public void Open()
        //{
        //    if (IsOpen)
        //    {
        //        throw new InvalidOperationException();
        //    }

        //    IsOpen = true;
        //}

        //public void CloseClose()
        //{
        //    if (!IsOpen)
        //    {
        //        throw new InvalidOperationException();
        //    }

        //    IsOpen = false;
        //}

        //public void MakeDeposit(decimal amount)
        //{
        //    if (!IsOpen)
        //    {
        //        throw new InvalidOperationException();
        //    }

        //    Balance += amount;
        //    WriteHistory(ActionType.Deposit, amount);
        //}

        //public void MakeLoan(decimal amount)
        //{
        //    if (!IsOpen)
        //    {
        //        throw new InvalidOperationException();
        //    }

        //    Balance -= amount;
        //    WriteHistory(ActionType.Loan, amount);
        //}

        //private void WriteHistory(ActionType type, decimal amount)
        //{
        //    var action = new AccountAction
        //    {
        //        Date = DateTime.Now,
        //        Type = type,
        //        Amount = amount
        //    };
        //    history.Add(action);
        //}
    }

    public sealed class BankTerminal
    {
        private const int INVALID_OPTION = -1;

        private readonly Bank bank;

        public BankTerminal(Bank bank)
        {
            this.bank = bank;
        }

        public void ShowBankMenu()
        {
            var stop = false;
            do
            {
                Console.WriteLine();
                Console.WriteLine("Меню банка:");
                Console.WriteLine("1. Добавить клиента");
                Console.WriteLine("2. Показать всех клиентов");
                Console.WriteLine("3. Выбрать клиента");
                Console.WriteLine("0. Выход");

                int option = GetMenuOption();
                switch (option)
                {
                    case 1:
                        AddClient();
                        break;
                    case 2:
                        ShowClients();
                        break;
                    case 3:
                        SelectClient();
                        break;
                    case 0:
                        stop = true;
                        break;
                    default:
                        Console.WriteLine("Введите значение меню из списка");
                        break;
                }
            }
            while (!stop);
        }

        private void AddClient()
        {
            var client = new Client
            {
                FullName = ReadText("ФИО клиента: "),
                Age = ReadAge("Возраст: "),
                WorkPlace = ReadText("Место работы: ")
            };
            bank.AddClient(client);
            Console.WriteLine($"Клиент №{client.ID} {client.FullName} был успешно добавлен");
        }

        private void ShowClients()
        {
            List<Client> clients = bank.GetClients();
            foreach (Client client in clients)
            {
                Console.WriteLine($"{client.ID:d3} {client.FullName}. Возраст: {client.Age}. Место работы: {client.WorkPlace}.");
            }
            Console.WriteLine($"Всего клиентов: {clients.Count}");
        }

        private void SelectClient()
        {
            ulong clientID = ReadID("Номер клиента: ");
            try
            {
                Client client = bank.GetClient(clientID);
                ShowClientMenu(client);
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Клиента с таким номер не существует");
            }
        }

        private void ShowClientMenu(Client client)
        {
            var stop = false;
            do
            {
                Console.WriteLine();
                Console.WriteLine($"Меню клиента №{client.ID} {client.FullName}:");
                Console.WriteLine("1. Редактировать клиента");
                Console.WriteLine("2. Удалить клиента");
                Console.WriteLine("3. Информация о счетах");
                Console.WriteLine("4. Выбрать счет");
                Console.WriteLine("5. Открыть новый счет");
                Console.WriteLine("0. Вернуться назад");

                int option = GetMenuOption();
                switch (option)
                {
                    case 1:
                        EditClient(client);
                        break;
                    case 2:
                        if (DeleteClient(client))
                        {
                            stop = true;
                        }
                        break;
                    case 3:
                        ShowAccounts(client);
                        break;
                    case 4:
                        SelectAccount(client);
                        break;
                    case 5:
                        OpenNewAccount(client);
                        break;
                    case 0:
                        stop = true;
                        break;
                    default:
                        Console.WriteLine("Введите значение меню из списка");
                        break;
                }
            }
            while (!stop);
        }

        private void EditClient(Client client)
        {
            throw new NotImplementedException();
        }

        private bool DeleteClient(Client client)
        {
            if (ConfirmAction($"Вы действительно хотите удалить клиента №{client.ID} {client.FullName}?"))
            {
                bank.DeleteClient(client.ID);
                Console.WriteLine($"Клиент №{client.ID} {client.FullName} был успешно удален");
                return true;
            }

            return false;
        }

        private void ShowAccounts(Client client)
        {
            throw new NotImplementedException();
        }

        private void SelectAccount(Client client)
        {
            throw new NotImplementedException();
        }

        private void OpenNewAccount(Client client)
        {
            throw new NotImplementedException();
        }

        private void ShowAccountMenu(Account account)
        {
            var stop = false;
            do
            {
                string changeAccountStatus = account.IsOpen ? "Закрыть" : "Открыть";

                Console.WriteLine();
                Console.WriteLine($"Меню счёта №{account.ID}:");
                Console.WriteLine($"1. {changeAccountStatus} счёт");
                Console.WriteLine("2. Вклад денег");
                Console.WriteLine("3. Снятие денег");
                Console.WriteLine("4. Просмотр баланса");
                Console.WriteLine("5. Просмотр истории");
                Console.WriteLine("0. Вернуться назад");

                int option = GetMenuOption();
                switch (option)
                {
                    case 1:
                        if (account.IsOpen)
                        {
                            OpenAccount(account);
                        }
                        else
                        {
                            CloseAccount(account);
                        }
                        break;
                    case 2:
                        MakeLoan(account);
                        break;
                    case 3:
                        MakeDeposit(account);
                        break;
                    case 4:
                        ShowBalance(account);
                        break;
                    case 0:
                        stop = true;
                        break;
                    default:
                        Console.WriteLine("Введите значение меню из списка");
                        break;
                }
            }
            while (!stop);
        }

        private void OpenAccount(Account account)
        {
            throw new NotImplementedException();
        }

        private void CloseAccount(Account account)
        {
            throw new NotImplementedException();
        }

        private void MakeLoan(Account account)
        {
            throw new NotImplementedException();
        }

        private void MakeDeposit(Account account)
        {
            throw new NotImplementedException();
        }

        private void ShowBalance(Account account)
        {
            throw new NotImplementedException();
        }

        private string ReadText(string label)
        {
            string text;
            do
            {
                Console.Write(label);
                text = Console.ReadLine().Trim();
            }
            while (string.IsNullOrEmpty(text) /*&& Char.IsDigit(e.KeyChar) == true*/); //Проверка на ввод только букв
            return text;
        }

        private uint ReadAge(string label)
        {
            uint age;
            string input;
            do
            {
                Console.Write(label);
                input = Console.ReadLine();
            }
            while (!uint.TryParse(input, out age) || age == 0);
            return age;
        }

        private ulong ReadID(string label)
        {
            ulong id;
            string input;
            do
            {
                Console.Write(label);
                input = Console.ReadLine();
            }
            while (!ulong.TryParse(input, out id) || id == 0);
            return id;
        }

        private bool ConfirmAction(string label)
        {
            Console.WriteLine($"{label} (y - да)");
            string input = Console.ReadLine();
            return input == "y" || input == "Y";
        }

        private int GetMenuOption()
        {
            string input = Console.ReadLine();

            int option;
            if (!int.TryParse(input, out option))
            {
                option = INVALID_OPTION;
            }

            return option;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var bank = new Bank();
            var terminal = new BankTerminal(bank);
            terminal.ShowBankMenu();
        }
    }
}