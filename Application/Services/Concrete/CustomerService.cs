using Application.Services.Abstract;
using Core.Constants;
using Core.Entities;
using Data.UnitOfWork.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Services.Concrete;
public class CustomerService : ICustomerService
{
    
    private readonly UnitOfWork _unitOfWork;
    public CustomerService(UnitOfWork unitOfWork)
    { 
        _unitOfWork = unitOfWork;
    }
    public Customer CheckingCustomer()
    {
        PasswordHasher<Customer> _passwordHasher = new PasswordHasher<Customer>();
        Messages.InputMessage("your email");
        string email = Console.ReadLine();
        Customer customer = _unitOfWork.Customers.GetAll().FirstOrDefault(x => x.Email == email);
        Messages.InputMessage("your password");
        string password = Console.ReadLine();
        if (customer == null)
            return null;
        var verificationResult = _passwordHasher.VerifyHashedPassword(customer, customer.Password, password);
        if (verificationResult == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Failed || email != customer.Email)
        {

            return null;
        }
        return customer;
    }
    public void BuyProduct(int customerId) 
    {
        InputId:
        bool anyProduct = false;
        List<Product> products = _unitOfWork.Products.GetAll();
        foreach (Product product in products)
        {
            if (!product.IsDeleted)
            {
                anyProduct = true;
                Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Count: {product.Count}, Price: {product.Price}");
            }
        }
        if (!anyProduct)
        {
            Messages.NotFoundMessage("Product");
            return;
        }
        Messages.InputMessage("product id");
        string inputId = Console.ReadLine();
        bool isSuceeded = int.TryParse(inputId, out int id);
        if(!isSuceeded)
        {
            Messages.InvalidInputMessage("id");
            goto InputId;
        }
        bool isAnyProduct = false;
        List<Product> _products = _unitOfWork.Products.GetAll();
        foreach (Product product in _products)
        {
            if (!product.IsDeleted && product.Id == id)
            {
                anyProduct = true;
            CountInput:
                Messages.InputMessage("count");
                string inputCount = Console.ReadLine();
                int count;
                bool isSucceededCount = int.TryParse(inputCount, out count);
                if (product.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The product finished in our stock, sorry!");
                    return;
                }
                if (!isSucceededCount || count>product.Count)
                {
                    Messages.InvalidInputMessage("count");
                    goto CountInput;
                }
                product.Count = product.Count - count;
                Order order = new Order()
                {
                    Name = " ",
                    ProductId = product.Id,
                    Product = product,
                    TotalPrice = count*product.Price,
                    CustomerId = customerId,
                    Customer = _unitOfWork.Customers.Get(customerId),
                    SellerId = product.SellerId,
                    Seller = product.Seller,
                    
                };
                _unitOfWork.Orders.Add(order);
                _unitOfWork.CommitWithoutMessage();

                order.Name = $"Order NO:{order.Id}";
                _unitOfWork.Orders.Update(order);
                _unitOfWork.Commit();


            }
        }
        if (!anyProduct)
            Messages.NotFoundMessage("Product");
        
    }
    public void GetAllOrders(int customerId)
    {
        List<Order> orders = _unitOfWork.Orders.GetAll();
        foreach (Order order in orders)
        {
            if (order.CustomerId == customerId && order.IsDeleted == false)
            {
                Console.WriteLine($"{order.Name}, Product: {order.Product.Name}, Price: {order.TotalPrice}, Seller: {order.Product.Seller.Name} {order.Product.Seller.Surname}");
            }
        }
    }
    public void GetAllOrdersOfDate(int customerId) 
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
            if (!order.IsDeleted && order.CreatedDate.Date == wantedDate.Date && order.CustomerId == customerId)
            {
                anyOrder = true;
                Console.WriteLine($"Id: {order.Id}, Name: {order.Name}, Price: {order.TotalPrice}, Seller: {order.Seller.Name}");
            }
        }
        if (!anyOrder)
            Messages.NotFoundMessage("Order");
    }
    public void FilterProducts() 
    {
    InputFilterName:
        Messages.InputMessage("the name for filter");
        string name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
        {
            Messages.InvalidInputMessage("name");
            goto InputFilterName;
        }
        bool anyProduct = false;
        List<Product> products = _unitOfWork.Products.GetAll();
        foreach (Product product in products)
        {
            if (!product.IsDeleted && product.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
            {
                anyProduct = true;
                Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Count: {product.Count}, Price: {product.Price}");
            }
        }
        if (!anyProduct)
            Messages.NotFoundMessage("Product");
    }
}
