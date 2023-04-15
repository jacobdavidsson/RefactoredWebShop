using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace WebShopCleanCode
{
    public class WebShop
    {
        bool running = true;
        Database database = Database.GetInstance();
        List<Product> products = new List<Product>();
        List<Customer> customers = new List<Customer>();

        string currentMenu = "main menu";
        int currentChoice = 1;
        int amountOfOptions = 3;
        string option1 = "See Wares";
        string option2 = "Customer Info";
        string option3 = "Login";
        string option4 = "";
        string info = "What would you like to do?";

        string username = null;
        string password = null;
        Customer currentCustomer;

        public WebShop()
        {
            products = database.GetProducts();
            customers = database.GetCustomers();
        }

        private string UserDetailsInput(string value)
        {
            string choice = "";
            bool next = false;
            string newValue = null;

            while (true)
            {
                Console.WriteLine($"Do you want a {value}? y/n");
                choice = Console.ReadLine();
                if (choice.Equals("y"))
                {
                    while (true)
                    {
                        Console.WriteLine($"Please write your {value}.");
                        newValue = Console.ReadLine();
                        if (newValue.Equals(""))
                        {
                            TextOutput("Please actually write something");
                            continue;
                        }
                        else
                        {
                            next = true;
                            break;
                        }
                    }
                }
                if (choice.Equals("n") || next)
                {
                    next = false;
                    break;
                }
                TextOutput("y or n, please.");
            }
            return newValue;
        }
        private void LoginMenuOptions()
        {
            option1 = "Set Username";
            option2 = "Set Password";
            option3 = "Login";
            option4 = "Register";
            amountOfOptions = 4;
            info = "Please submit username and password.";
            currentChoice = 1;
            currentMenu = "login menu";
        }
        private void TextOutput(string text)
        {
            Console.WriteLine();
            Console.WriteLine(text);
            Console.WriteLine();
        }
        private void Sort(string type, bool sorted) 
        {
            bubbleSort(type, sorted);
            Console.WriteLine();
            Console.WriteLine("Wares sorted.");
            Console.WriteLine();
        }
        private void DisplayItems()
        {
            for (int i = 0; i < amountOfOptions; i++)
            {
                Console.WriteLine($"{i + 1}: {products[i].Name}, {products[i].Price}kr");
            }
            Console.WriteLine($"Your funds: {currentCustomer.Funds}");
        }
        private void DisplayOptions()
        {
            Console.WriteLine($"1: {option1}");
            Console.WriteLine($"2: {option2}");
            if (amountOfOptions > 2)
            {
                Console.WriteLine($"3: {option3}");
            }
            if (amountOfOptions > 3)
            {
                Console.WriteLine($"4: {option4}");
            }
        }
        private void DisplayButtonOptions()
        {
            for (int i = 0; i < amountOfOptions; i++)
            {
                Console.Write($"{i + 1}\t");
            }
            Console.WriteLine();
            for (int i = 1; i < currentChoice; i++)
            {
                Console.Write("\t");
            }
            Console.WriteLine("|");
            Console.WriteLine("Your buttons are Left, Right, OK, Back and Quit.");
        }
        private void DisplayCurrentUser()
        {
            if (currentCustomer != null)
            {
                Console.WriteLine($"Current user: {currentCustomer.Username}");
            }
            else
            {
                Console.WriteLine("Nobody logged in.");
            }
        }
        private void WaresMenu() 
        {
            option1 = "See all wares";
            option2 = "Purchase a ware";
            option3 = "Sort wares";
            if (currentCustomer == null)
            {
                option4 = "Login";
            }
            else
            {
                option4 = "Logout";
            }
            amountOfOptions = 4;
            currentChoice = 1;
            currentMenu = "wares menu";
            info = "What would you like to do?";
        }
        private void WaresMenuLoggedIn()
        {
            option1 = "See Wares";
            option2 = "Customer Info";
            if (currentCustomer == null)
            {
                option3 = "Login";
            }
            else
            {
                option3 = "Logout";
            }
            info = "What would you like to do?";
            currentMenu = "main menu";
            currentChoice = 1;
            amountOfOptions = 3;
        }
        private void MainMenu()
        {
            switch (currentChoice)
            {
                case 1:
                    WaresMenu();
                    break;
                case 2:
                    if (currentCustomer != null)
                    {
                        option1 = "See your orders";
                        option2 = "See your info";
                        option3 = "Add funds";
                        option4 = "";
                        amountOfOptions = 3;
                        currentChoice = 1;
                        info = "What would you like to do?";
                        currentMenu = "customer menu";
                    }
                    else
                    {
                        TextOutput("Nobody is logged in");
                    }
                    break;
                case 3:
                    if (currentCustomer == null)
                    {
                        username = null;
                        password = null;
                        LoginMenuOptions();
                    }
                    else
                    {
                        option3 = "Login";
                        TextOutput($"{currentCustomer.Username} logged out.");
                        currentChoice = 1;
                        currentCustomer = null;
                    }
                    break;
                default:
                    TextOutput("Not an option");
                    break;
            }
        }
        private void CustomerMenu()
        {
            switch (currentChoice)
            {
                case 1:
                    currentCustomer.PrintOrders();
                    break;
                case 2:
                    currentCustomer.PrintInfo();
                    break;
                case 3:
                    Console.WriteLine("How many funds would you like to add?");
                    string amountString = Console.ReadLine();
                    try
                    {
                        int amount = int.Parse(amountString);
                        if (amount < 0)
                        {
                            TextOutput("Don't add negative amounts");
                        }
                        else
                        {
                            currentCustomer.Funds += amount;
                            TextOutput($"{amount} added to your profile.");
                        }
                    }
                    catch (FormatException e)
                    {
                        TextOutput("Please write a number next time.");
                    }
                    break;
                default:
                    TextOutput("Not an option.");
                    break;
            }
        }
        private void SortMenu()
        {
            bool back = true;
            switch (currentChoice)
            {
                case 1:
                    Sort("name", false);
                    break;
                case 2:
                    Sort("name", true);
                    break;
                case 3:
                    Sort("price", false);
                    break;
                case 4:
                    Sort("price", true);
                    break;
                default:
                    back = false;
                    TextOutput("Not an option");
                    break;
            }
            if (back)
            {
                WaresMenu();
            }
        }
        private void PurchaseWaresMenu()
        {
            switch (currentChoice)
            {
                case 1:
                    Console.WriteLine();
                    foreach (Product product in products)
                    {
                        product.PrintInfo();
                    }
                    Console.WriteLine();
                    break;
                case 2:
                    if (currentCustomer != null)
                    {
                        currentMenu = "purchase menu";
                        info = "What would you like to purchase?";
                        currentChoice = 1;
                        amountOfOptions = products.Count;
                    }
                    else
                    {
                        TextOutput("You must be logged in to purchase wares");
                        currentChoice = 1;
                    }
                    break;
                case 3:
                    option1 = "Sort by name, descending";
                    option2 = "Sort by name, ascending";
                    option3 = "Sort by price, descending";
                    option4 = "Sort by price, ascending";
                    info = "How would you like to sort them?";
                    currentMenu = "sort menu";
                    currentChoice = 1;
                    amountOfOptions = 4;
                    break;
                case 4:
                    if (currentCustomer == null)
                    {
                        LoginMenuOptions();
                    }
                    else
                    {
                        option4 = "Login";
                        TextOutput($"{currentCustomer.Username} logged out");
                        currentCustomer = null;
                        currentChoice = 1;
                    }
                    break;
                case 5:
                    break;
                default:
                    TextOutput("Not an option.");
                    break;
            }
        }
        private void LogInMenu()
        {
            switch (currentChoice)
            {
                case 1:
                    SetUserName();
                    break;
                case 2:
                    SetPassword();
                    break;
                case 3:
                    LogIn();
                    break;
                case 4:
                    Register();
                    break;
                default:
                    TextOutput("Not an option");
                    break;
            }
        }
        private void LogIn() 
        {
            if (username == null || password == null)
            {
                TextOutput("Incomplete data");
            }
            else
            {
                bool found = false;
                foreach (Customer customer in customers)
                {
                    if (username.Equals(customer.Username) && customer.CheckPassword(password))
                    {
                        TextOutput($"{customer.Username} logged in.");
                        currentCustomer = customer;
                        found = true;
                        WaresMenuLoggedIn();
                        break;
                    }
                }
                if (found == false)
                {
                    TextOutput("Invalid credentials");
                }
            }
        }
        private void Register()
        {
            Console.WriteLine("Please write your username.");
            string newUsername = Console.ReadLine();
            foreach (Customer customer in customers)
            {
                if (customer.Username.Equals(username))
                {
                    TextOutput("Username already exists");
                    break;
                }
            }

            string newPassword;
            string firstName;
            string lastName;
            string email;
            string age;
            string address;
            string phoneNumber;

            newPassword = UserDetailsInput("password");
            firstName = UserDetailsInput("first name");
            lastName = UserDetailsInput("last name");
            email = UserDetailsInput("email");
            age = UserDetailsInput("age");
            address = UserDetailsInput("address");
            phoneNumber = UserDetailsInput("phone number");

            Customer newCustomer = new Customer(newUsername, newPassword, firstName, lastName, email, age, address, phoneNumber);
            customers.Add(newCustomer);
            currentCustomer = newCustomer;
            TextOutput($"{newCustomer.Username} successfully added and is now logged in.");
            WaresMenuLoggedIn();
        }
        private void SetUserName()
        {
            Console.WriteLine("A keyboard appears.");
            Console.WriteLine("Please input your username.");
            username = Console.ReadLine();
            Console.WriteLine();
        }
        private void SetPassword()
        {
            Console.WriteLine("A keyboard appears.");
            Console.WriteLine("Please input your password.");
            password = Console.ReadLine();
            Console.WriteLine();
        }
        private void Back() 
        {
            if (currentMenu.Equals("main menu"))
            {
                TextOutput("You're already on the main menu.");
            }
            else if (currentMenu.Equals("purchase menu"))
            {
                WaresMenu();
            }
            else
            {
                WaresMenuLoggedIn();
            }
        }
        private void BuyItems()
        {
            int index = currentChoice - 1;
            Product product = products[index];
            if (product.InStock())
            {
                if (currentCustomer.CanAfford(product.Price))
                {
                    currentCustomer.Funds -= product.Price;
                    product.NrInStock--;
                    currentCustomer.Orders.Add(new Order(product.Name, product.Price, DateTime.Now));
                    TextOutput($"Successfully bought {product.Name}");
                }
                else
                {
                    TextOutput("You cannot afford.");
                }
            }
            else
            {
                TextOutput("Not in stock.");
            }
        }

        public void Run()
        {
            Console.WriteLine("Welcome to the WebShop!");
            while (running)
            {
                Console.WriteLine(info);

                if (currentMenu.Equals("purchase menu"))
                {
                    DisplayItems();
                }
                else
                {
                    DisplayOptions();
                }

                DisplayButtonOptions();
                DisplayCurrentUser();

                string choice = Console.ReadLine().ToLower();
                switch (choice)
                {
                    case "left":
                        if (currentChoice > 1)
                        {
                            currentChoice--;
                        }
                        break;
                    case "right":
                        if (currentChoice < amountOfOptions)
                        {
                            currentChoice++;
                        }
                        break;
                    case "ok":
                        if (currentMenu.Equals("main menu"))
                        {
                            MainMenu();
                        }
                        else if (currentMenu.Equals("customer menu"))
                        {
                            CustomerMenu();
                        }
                        else if (currentMenu.Equals("sort menu"))
                        {
                            SortMenu();
                        }
                        else if (currentMenu.Equals("wares menu"))
                        {
                            PurchaseWaresMenu();
                        }
                        else if (currentMenu.Equals("login menu"))
                        {
                            LogInMenu();
                        }
                        else if (currentMenu.Equals("purchase menu"))
                        {
                            BuyItems();
                        }
                        break;
                    case "back":
                        Back();
                        break;
                    case "quit":
                        Console.WriteLine("The console powers down. You are free to leave.");
                        return;
                    default:
                        Console.WriteLine("That is not an applicable option.");
                        break;
                }
            }
        }
        private void bubbleSort(string variable, bool ascending)
        {
            if (variable.Equals("name")) {
                int length = products.Count;
                for(int i = 0; i < length - 1; i++)
                {
                    bool sorted = true;
                    int length2 = length - i;
                    for (int j = 0; j < length2 - 1; j++)
                    {
                        if (ascending)
                        {
                            if (products[j].Name.CompareTo(products[j + 1].Name) < 0)
                            {
                                Product temp = products[j];
                                products[j] = products[j + 1];
                                products[j + 1] = temp;
                                sorted = false;
                            }
                        }
                        else
                        {
                            if (products[j].Name.CompareTo(products[j + 1].Name) > 0)
                            {
                                Product temp = products[j];
                                products[j] = products[j + 1];
                                products[j + 1] = temp;
                                sorted = false;
                            }
                        }
                    }
                    if (sorted == true)
                    {
                        break;
                    }
                }
            }
            else if (variable.Equals("price"))
            {
                int length = products.Count;
                for (int i = 0; i < length - 1; i++)
                {
                    bool sorted = true;
                    int length2 = length - i;
                    for (int j = 0; j < length2 - 1; j++)
                    {
                        if (ascending)
                        {
                            if (products[j].Price > products[j + 1].Price)
                            {
                                Product temp = products[j];
                                products[j] = products[j + 1];
                                products[j + 1] = temp;
                                sorted = false;
                            }
                        }
                        else
                        {
                            if (products[j].Price < products[j + 1].Price)
                            {
                                Product temp = products[j];
                                products[j] = products[j + 1];
                                products[j + 1] = temp;
                                sorted = false;
                            }
                        }
                    }
                    if (sorted == true)
                    {
                        break;
                    }
                }
            }
        }
    }
}
