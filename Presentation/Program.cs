using Application.Services.Concrete;
using Core.Constants;
using Microsoft.AspNetCore.Identity;
using Data.UnitOfWork.Concrete;
using Core.Entities;
using Data;
using Data.Context;


namespace ConsoleCommerceApplication;
class Program
{
    private static readonly UnitOfWork _unitOfWork;
    public static void Main()
    {
        UnitOfWork _unitOfWork = new UnitOfWork();
        AppDbContext context = new AppDbContext();
        DbInitializer.Initialize(context);
        AdminService adminService = new AdminService(_unitOfWork);
        SellerService sellerService = new SellerService(_unitOfWork);
        CustomerService customerService = new CustomerService(_unitOfWork);
        MainMenuSection:
        MainMenu();
        string level = Console.ReadLine();
        if (level.ToLower() != "customer" && level.ToLower() != "seller" && level.ToLower() != "admin" && level.ToLower() != "exit")
            Messages.InvalidInputMessage("level");
        if(level.ToLower() == "exit")
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Program finished, thank you for using our app!");
            return;
        }
        if (level.ToLower() == "admin")
        {
            Admin admin = adminService.CheckingAdmin();
            if (admin == null)
            {
                Console.WriteLine("Your password is incorrect!");
                return;
            }
            bool exit = true;
            while (exit)
            {
                AdminMenu();
                string inputChoice = Console.ReadLine();
                bool isSuceeded = int.TryParse(inputChoice, out int choice);
                if (!isSuceeded)
                    Messages.InvalidInputMessage("choice");
                switch (choice)
                {
                    case (int)AdminOperations.Exit:
                        exit = false;
                        goto MainMenuSection;
                    case (int)AdminOperations.CreateSeller:
                        adminService.CreateSeller();
                        break;
                    case (int)AdminOperations.CreateCustomer:
                        adminService.CreateCustomer();
                        break;
                    case (int)AdminOperations.DeleteSeller:
                        adminService.DeleteSeller();
                        break;
                    case (int)AdminOperations.DeleteCustomer:
                        adminService.DeleteCustomer();
                        break;
                    case (int)AdminOperations.GetAllCustomers:
                        adminService.GetAllCustomers();
                        break;
                    case (int)AdminOperations.GetAllSellers:
                        adminService.GetAllSellers();
                        break;
                    case (int)AdminOperations.AddCategory:
                        adminService.AddCategory();
                        break;
                    case (int)AdminOperations.GetAllOrders:
                        adminService.GetAllOrders();
                        break;
                    case (int)AdminOperations.GetAllOrdersOfCustomer:
                        adminService.GetAllOrdersOfCustomer();
                        break;
                    case (int)AdminOperations.GetAllOrdersOfSeller:
                        adminService.GetAllOrdersOfSeller();
                        break;
                    case (int)AdminOperations.GetAllOrdersOfDate:
                        adminService.GetAllOrdersOfDate();
                        break;
                    default:
                        Messages.InvalidInputMessage("choice");
                        break;
                }
            }
        }

        if (level.ToLower() == "customer")
        {
            Customer customer = customerService.CheckingCustomer();
            if (customer == null)
            {
                Console.WriteLine("Your email or password is incorrect");
                return;
            }

            bool exit = true;
            while (exit)
            {
                CustomerMenu();
                string inputChoice = Console.ReadLine();
                bool isSuceeded = int.TryParse(inputChoice, out int choice);
                if (!isSuceeded)
                    Messages.InvalidInputMessage("choice");
                switch (choice)
                {
                    case (int)CustomerOperations.Exit:
                        exit = false;
                        goto MainMenuSection;
                    case (int)CustomerOperations.BuyProduct:
                        customerService.BuyProduct(customer.Id);
                        break;
                    case (int)CustomerOperations.GetAllOrders:
                        customerService.GetAllOrders(customer.Id);
                        break;
                    case (int)CustomerOperations.GetAllOrdersOfTime:
                        customerService.GetAllOrdersOfDate(customer.Id);
                        break;
                    case (int)CustomerOperations.FilterProducts:
                        customerService.FilterProducts();
                        break;
                    default:
                        Messages.InvalidInputMessage("choice");
                        break;
                }
            }
        }

        if (level.ToLower() == "seller")
        {
            Seller seller = sellerService.CheckingSeller();
            if (seller == null)
            {
                Console.WriteLine("Your email or password is incorrect");
                return;
            }


            bool exit = true;
            while (exit)
            {
                SellerMenu();
                string inputChoice = Console.ReadLine();
                bool isSuceeded = int.TryParse(inputChoice, out int choice);
                if (!isSuceeded)
                    Messages.InvalidInputMessage("choice");
                switch (choice)
                {
                    case (int)SellerOperations.Exit:
                        goto MainMenuSection;
                    case (int)SellerOperations.AddProduct:
                        sellerService.AddProduct(seller.Id);
                        break;
                    case (int)SellerOperations.UpdateCountOfProduct:
                        sellerService.UpdateCountOfProduct(seller.Id);
                        break;
                    case (int)SellerOperations.DeleteProduct:
                        sellerService.DeleteProduct(seller.Id);
                        break;
                    case (int)SellerOperations.GetAllProductsOfDate:
                        sellerService.GetAllProductsOfDate();
                        break;
                    case (int)SellerOperations.FilterProductByName:
                        sellerService.FilterProductByName();
                        break;
                    case (int)SellerOperations.GetTotalIncome:
                        sellerService.GetTotalIncome(seller.Id);
                        break;
                    default:
                        Messages.InvalidInputMessage("choice");
                        break;
                }
            }
        }
    }
    public static void MainMenu()
    {

        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("Welcome back, dear User");
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("At first you must login for doing some works. Please choose your level: Customer, Seller or Admin");
        Console.WriteLine("For exiting please write exit");
    }
    public static void AdminMenu()
    {

        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("Welcome back, dear Admin");
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("-----Menu-----");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("1. Create Seller");
        Console.WriteLine("2. Create Customer");
        Console.WriteLine("3. Delete Seller");
        Console.WriteLine("4. Delete Customer");
        Console.WriteLine("5. Get All Customers");
        Console.WriteLine("6. Get All Sellers");
        Console.WriteLine("7. Add Category");
        Console.WriteLine("8. Get All Orders");
        Console.WriteLine("9. Get All Orders Of Customer");
        Console.WriteLine("10. Get All Orders Of Seller");
        Console.WriteLine("11. Get All Orders By Date");
        Console.WriteLine("0. Exit");


    }
    public static void SellerMenu()
    {

        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("Welcome back, dear Seller");
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("-----Menu-----");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("1. Add Product");
        Console.WriteLine("2. Update Count Of Product");
        Console.WriteLine("3. Delete Product");
        Console.WriteLine("4. Get All Products Of Date");
        Console.WriteLine("5. Filter Product By Name");
        Console.WriteLine("6. Get Total Income");
        Console.WriteLine("0. Exit");
    }
    public static void CustomerMenu()
    {

        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("Welcome back, dear Customer");
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("-----Menu-----");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("1. Buy Product");
        Console.WriteLine("2. Get All Orders");
        Console.WriteLine("3. Get All Orders By Date");
        Console.WriteLine("4. Filter Products");
        Console.WriteLine("0. Exit");
    }
}
