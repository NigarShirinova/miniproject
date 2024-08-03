using Application.Services.Abstract;
using Azure.Messaging;
using Core.Constants;
using Core.Entities;
using Data.Repositories.Base;
using Data.UnitOfWork.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services.Concrete;
public class AdminService : IAdminService
{
    private readonly UnitOfWork _unitOfWork;
    public AdminService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public Admin CheckingAdmin()
    {
        PasswordHasher<Admin> _passwordHasher = new PasswordHasher<Admin>();
        Admin admin = _unitOfWork.Admins.GetAll().FirstOrDefault();
        if (admin == null)
        {
            Messages.NotFoundMessage("admin");
            return null;
        }
        Messages.InputMessage("your password");
        string password = Console.ReadLine();
        var verificationResult = _passwordHasher.VerifyHashedPassword(admin, admin.Password, password);
        if (verificationResult == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Failed)
        {

            return null;
        }
        return admin;
    }
    public void CreateSeller() 
    {
    NameInput:
        Messages.InputMessage("name");
        string name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
        {
            Messages.InvalidInputMessage("Name");
            goto NameInput;
        }
    SurNameInput:
        Messages.InputMessage("surname");
        string surname = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(surname))
        {
            Messages.InvalidInputMessage("Surname");
            goto SurNameInput;
        }
    EmailInput:
        Messages.InputMessage("email");
        string email = Console.ReadLine();
        if(!email.IsValidEmail())
        {
            Messages.InvalidInputMessage("email");
            goto EmailInput;
        }
    PasswordInput:
        Messages.InputMessage("password");
        Messages.PasswordFormMessage();
        string password = Console.ReadLine();
        if (!password.IsValidPassword())
        {
            Messages.InvalidInputMessage("password");

            goto PasswordInput;
        }
    PhoneNumberInput:
        Messages.InputMessage("phone number");
        string phoneNumber = Console.ReadLine();
        if (!phoneNumber.IsValidPhoneNumber())
        {
            Messages.InvalidInputMessage("phone number");
            goto PhoneNumberInput;
        }
    PinInput:
        Messages.InputMessage("pin");
        string pin = Console.ReadLine();
        if (!pin.IsValidPIN())
        {
            Messages.InvalidInputMessage("pin");
            goto PinInput;
        }
    SeriaInput:
        Messages.InputMessage("seria");
        string seria = Console.ReadLine();
        if(seria.ToLower()!= "aze" && seria.ToLower() != "aa")
        {
            Messages.InvalidInputMessage("seria");
            goto SeriaInput;
        }
    SeriaNumberInput:
        Messages.InputMessage("seria number");
        string seriaNumber = Console.ReadLine();
        if(!int.TryParse(seriaNumber, out int value) || seriaNumber.Length!= 7 || seriaNumber.IsNullOrEmpty())
        {
            Messages.InvalidInputMessage("seria number");
            goto SeriaNumberInput;
        }
        PasswordHasher<Seller> passwordHasher = new PasswordHasher<Seller>();

        Seller seller = new Seller
        {
            Name = name,
            Surname = surname,
            Email = email,
            PhoneNumber = phoneNumber,
            PIN = pin,
            Seria = seria,
            SeriaNumber = seriaNumber,
            CreatedDate = DateTime.Now,            
        };

        seller.Password = passwordHasher.HashPassword(seller, password);
        _unitOfWork.Sellers.Add(seller);
        _unitOfWork.Commit();
    }
    public void CreateCustomer()
    {
    NameInput:
        Messages.InputMessage("name");
        string name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
        {
            Messages.InvalidInputMessage("Name");
            goto NameInput;
        }
    SurNameInput:
        Messages.InputMessage("surname");
        string surname = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(surname))
        {
            Messages.InvalidInputMessage("Surname");
            goto SurNameInput;
        }
    EmailInput:
        Messages.InputMessage("email");
        string email = Console.ReadLine();
        if (!email.IsValidEmail())
        {
            Messages.InvalidInputMessage("email");
            goto EmailInput;
        }
    PasswordInput:
        Messages.InputMessage("password");
        Messages.PasswordFormMessage();
        string password = Console.ReadLine();
        if (!password.IsValidPassword())
        {
            Messages.InvalidInputMessage("password");

            goto PasswordInput;
        }
    PhoneNumberInput:
        Messages.InputMessage("phone number");
        string phoneNumber = Console.ReadLine();
        if (!phoneNumber.IsValidPhoneNumber())
        {
            Messages.InvalidInputMessage("phone number");
            goto PhoneNumberInput;
        }
    PinInput:
        Messages.InputMessage("pin");
        string pin = Console.ReadLine();
        if (!pin.IsValidPIN())
        {
            Messages.InvalidInputMessage("pin");
            goto PinInput;
        }
    SeriaInput:
        Messages.InputMessage("seria");
        string seria = Console.ReadLine();
        if (seria.ToLower() != "aze" && seria.ToLower() != "aa")
        {
            Messages.InvalidInputMessage("seria");
            goto SeriaInput;
        }
    SeriaNumberInput:
        Messages.InputMessage("seria number");
        string seriaNumber = Console.ReadLine();
        if (!int.TryParse(seriaNumber, out int value) || seriaNumber.Length != 7 || seriaNumber.IsNullOrEmpty())
        {
            Messages.InvalidInputMessage("seria number");
            goto SeriaNumberInput;
        }
        PasswordHasher<Customer> passwordHasher = new PasswordHasher<Customer>();

        Customer customer = new Customer();
        {
            customer.Name = name;
            customer.Email = email;
            customer.Surname = surname;
            customer.PIN = pin;
            customer.Seria = seria;
            customer.SeriaNumber = seriaNumber;
            customer.PhoneNumber = phoneNumber;
        };

        customer.Password = passwordHasher.HashPassword(customer, password);
        _unitOfWork.Customers.Add(customer);
        _unitOfWork.Commit();
    }
    public void DeleteSeller() 
    {
        GetAllSellers();    
        Messages.InputMessage("id");
    IdInput:
        string inputId = Console.ReadLine();
        int id;
        bool isSuceeded = int.TryParse(inputId, out id);
        if (!isSuceeded)
        {
            Messages.NotFoundMessage("Id");
            goto IdInput;
        }
        Seller seller = _unitOfWork.Sellers.Get(id);
        seller.IsDeleted = true;
        Messages.SuccessMessage();
    }
    public void DeleteCustomer() 
    {
        GetAllCustomers();
    IdInput:
        Messages.InputMessage("id");
        string inputId = Console.ReadLine();
        int id;
        bool isSuceeded = int.TryParse(inputId, out id);
        if (!isSuceeded)
        {
            Messages.NotFoundMessage("Id");
            goto IdInput;
        }
        Customer customer = _unitOfWork.Customers.Get(id);
        customer.IsDeleted = true;
        Messages.SuccessMessage();
    }
    public void GetAllCustomers() 
    { 
        bool anyCustomer = false;
        List<Customer> customers = _unitOfWork.Customers.GetAll();
        foreach (Customer customer in customers)
        {
            if (!customer.IsDeleted)
            {
                anyCustomer = true;
                Console.WriteLine($"Id: {customer.Id}, Name: {customer.Name}, Surname: {customer.Surname}, Email: {customer.Email}, Phone Number: {customer.PhoneNumber}, PIN: {customer.PIN}, Seria and Number : {customer.Seria}{customer.SeriaNumber}");
            }
        }
        if (!anyCustomer)
            Messages.NotFoundMessage("Customer");
        
    }
    public void GetAllSellers() 
    { 
        bool anySellers = false;
        List<Seller> sellers = _unitOfWork.Sellers.GetAll();
        foreach(Seller seller in sellers)
        {
            if (!seller.IsDeleted)
            Console.WriteLine($"Id: {seller.Id}, Name: {seller.Name}, Surname: {seller.Surname}, Email: {seller.Email}, Phone Number: {seller.PhoneNumber}, PIN: {seller.PIN}, Seria and Number: {seller.Seria}{seller.SeriaNumber}");
        }

        Messages.NotFoundMessage("Seller");
    }
    public void AddCategory() 
    {
    InputCategoryName:
        Messages.InputMessage("category's name");
        string name = Console.ReadLine();
        if (name.IsNullOrEmpty())
        {
            Messages.InvalidInputMessage("Name");
            goto InputCategoryName;
        }
        DateTime createdDate = DateTime.Now;
        Category category = new Category
        {
            Name = name,
            CreatedDate = createdDate
        };
        _unitOfWork.Categories.Add(category);
        _unitOfWork.Commit();
    }
    public void GetAllOrders() 
    {
        bool anyOrder = false;
        List<Order> orders = _unitOfWork.Orders.GetAll();
        foreach (Order order in orders)
        {
            if (!order.IsDeleted)
            {
                anyOrder = true;
                Console.WriteLine($"Id: {order.Id}, Name: {order.Name}, Price: {order.TotalPrice}, Seller: {order.Seller.Name} {order.Seller.Surname}, Customer: {order.Customer.Name} {order.Customer.Surname}");
            }
        }
        if (!anyOrder)
            Messages.NotFoundMessage("Order");
    }
    public void GetAllOrdersOfCustomer() 
    {
        Console.ResetColor();
        GetAllCustomers();
        InputCustomerId:
        Messages.InputMessage("customer's id");
        string inputCustomerId = Console.ReadLine();
        bool isSuceededCustomer = int.TryParse(inputCustomerId, out int customerId);
        if (!isSuceededCustomer)
        {
            Messages.InvalidInputMessage("Customer's Id");
            goto InputCustomerId;
        }
        List<Order> orders = _unitOfWork.Orders.GetAll();
        bool orderFlag = false;
        foreach (Order order in orders)
        {
            if (order.CustomerId == customerId && order.IsDeleted == false)
            {
                Console.WriteLine($"{order.Name}, Product: {order.Product.Name}, Price: {order.TotalPrice}");
                orderFlag = true;
            }
        }
        if (!orderFlag)
            Messages.NotFoundMessage("order");
    }
    public void GetAllOrdersOfSeller() 
    {
        GetAllSellers();
    InputSellerId:
        Messages.InputMessage("seller's id");
        string inputSellerId = Console.ReadLine();
        bool isSuceededSeller = int.TryParse(inputSellerId, out int sellerId);
        bool sellerFlag = false;
        if (!isSuceededSeller)
        {
            Messages.InvalidInputMessage("Seller's Id");
            goto InputSellerId;
        }
        List<Order> orders = _unitOfWork.Orders.GetAll();
        foreach (Order order in orders)
        {
            if (order.SellerId == sellerId && order.IsDeleted == false)
            {
                Console.WriteLine($"{order.Name}, Product: {order.Product.Name}, Price: {order.TotalPrice}");
                sellerFlag = true;
            }
        }
        if (!sellerFlag)
            Messages.NotFoundMessage("seller");
    }
    public void GetAllOrdersOfDate() 
    {
    DateInput:
        Messages.InputMessage("date");
        string date = Console.ReadLine();
        DateTime wantedDate;
        bool isSuceededTime = DateTime.TryParse(date, out wantedDate);
        if (!isSuceededTime)
        {
            Messages.InvalidInputMessage("date");
            goto DateInput;
        }
        bool anyOrder = false;
        List<Order> orders = _unitOfWork.Orders.GetAll();
        foreach (Order order in orders)
        {
            if (!order.IsDeleted && order.CreatedDate.Date == wantedDate.Date)
            {
                anyOrder = true;
                Console.WriteLine($"Id: {order.Id}, Name: {order.Name}, Price: {order.TotalPrice}, Seller: {order.Seller.Name}");
            }
        }
        if (!anyOrder)
            Messages.NotFoundMessage("Order");
    }
}
