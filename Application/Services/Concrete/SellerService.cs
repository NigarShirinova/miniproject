using Application.Services.Abstract;
using Core.Constants;
using Core.Entities;
using Data.UnitOfWork.Concrete;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.Concrete;
public class SellerService : ISellerService
{
    private readonly UnitOfWork _unitOfWork;
    public SellerService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public Seller CheckingSeller()
    {
        PasswordHasher<Seller> _passwordHasher = new PasswordHasher<Seller>();
        Messages.InputMessage("your email");
        string email = Console.ReadLine();
        Seller seller = _unitOfWork.Sellers.GetAll().FirstOrDefault(x => x.Email == email);
        Messages.InputMessage("your password");
        string password = Console.ReadLine();
        if (seller == null)
            return null;     
        var verificationResult = _passwordHasher.VerifyHashedPassword(seller, seller.Password, password);
        if (verificationResult == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Failed && email != seller.Email)
        {

            return null;
        }
        return seller;
    }
    public void AddProduct(int sellerId) 
    {
      
    ProductNameInput:
        Messages.InputMessage("Product's name");
        string name = Console.ReadLine();
        if(string.IsNullOrWhiteSpace(name))
        {
            Messages.InvalidInputMessage("product's name");
            goto ProductNameInput;
        }
    ProductPriceInput:
        Messages.InputMessage("Product's Price");
        string inputPrice  = Console.ReadLine();
        decimal price;
        bool isSucceededPrice = decimal.TryParse(inputPrice, out price);
        if(!isSucceededPrice)
        {
            Messages.InvalidInputMessage("Price");
            goto ProductPriceInput;
        }
        DateTime createdDate = DateTime.Now;

    CategoryIdInput:
        List<Category> categories = _unitOfWork.Categories.GetAll();
        foreach( Category category in categories)
        {
            Console.WriteLine($"Id: {category.Id}, Name: {category.Name}");
        }
        Messages.InputMessage("category's id");
        string inputId = Console.ReadLine();
        int id;
        bool isSucceededId = int.TryParse(inputId, out id);
        if (!isSucceededId)
        {
            Messages.InvalidInputMessage("category's id");
            goto CategoryIdInput;
        }
        bool flagCategory = false;
        foreach( Category category in categories)
        {
            if(category.Id == id)
            {
                flagCategory = true;
                break;
            }
        }
        if(!flagCategory)
        {
            Messages.NotFoundMessage("Category");
            goto CategoryIdInput;
        }
    CountInput:
        Messages.InputMessage("count");
        string inputCount = Console.ReadLine();
        int count;
        bool isSucceededCount = int.TryParse(inputCount, out count);
        if(!isSucceededCount)
        {
            Messages.InvalidInputMessage("count");
            goto CountInput;
        }
        Product product = new Product()
        {
            Name = name,
            Price = price,
            CategoryId = id,
            Category =_unitOfWork.Categories.Get(id),
            Count = count,
            Seller = _unitOfWork.Sellers.Get(sellerId),
            SellerId = sellerId
    };
        _unitOfWork.Products.Add(product);
        _unitOfWork.Commit();

    }
    public void UpdateCountOfProduct(int sellerId) 
    {
        GetAllProductsOfSeller(sellerId);
        Messages.InputMessage("product's id");
    IdInput:
        string inputId = Console.ReadLine();
        int id;
        bool isSucceeded = int.TryParse(inputId, out id);
        if (!isSucceeded)
        {
            Messages.InvalidInputMessage("Product id");
            goto IdInput;
        }
        Product product = _unitOfWork.Products.Get(id);
        if (product == null)
        {
            Messages.NotFoundMessage("Product");
            goto IdInput;
        }

    CountInput:
        Messages.InputMessage("new count");
        string inputCount = Console.ReadLine();
        int count;
        bool isSucceededCount = int.TryParse(inputCount, out count);
        if (!isSucceededCount)
        {
            Messages.InvalidInputMessage("count");
            goto CountInput;
        }
        DateTime updatedTime = DateTime.Now;
        product.UpdatedDate = updatedTime;
        product.Count = count;
        _unitOfWork.Commit();
    }
    public void DeleteProduct(int sellerId)
    {
        GetAllProductsOfSeller(sellerId);
        Messages.InputMessage("id");
    IdInput:
        string inputId = Console.ReadLine();
        int id;
        bool isSuceeded = int.TryParse(inputId, out id);
        if (!isSuceeded)
        {
            Messages.NotFoundMessage("Product id");
            goto IdInput;
        }
        Product product = _unitOfWork.Products.Get(id);
        product.IsDeleted = true;
        _unitOfWork.Commit();
    }
    public void GetAllProductsOfSeller(int sellerId)
    {
        bool anyProduct = false;
        List<Product> products = _unitOfWork.Products.GetAll();
        foreach (Product product in products)
        {
            if (!product.IsDeleted && product.SellerId == sellerId)
            {
                anyProduct = true;
                Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Count: {product.Count}, Price: {product.Price}");
            }
        }
        if (!anyProduct)
            Messages.NotFoundMessage("Product");
    }
    public void GetAllProducts()
    {
        bool anyProduct = false;
        List<Product> products = _unitOfWork.Products.GetAll();
        foreach (Product product in products)
        {
            if (!product.IsDeleted )
            {
                anyProduct = true;
                Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Count: {product.Count}, Price: {product.Price}");
            }
        }
        if (!anyProduct)
            Messages.NotFoundMessage("Product");
    }
    public void GetAllProductsOfDate() 
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
        bool anyProduct = false;
        List<Product> products = _unitOfWork.Products.GetAll();
        foreach (Product product in products)
        {
            if (!product.IsDeleted && product.CreatedDate.Date == wantedDate.Date)
            {
                anyProduct = true;
                Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Count: {product.Count}, Price: {product.Price}");
            }
        }
        if (!anyProduct)
            Messages.NotFoundMessage("Product");
    }
    public void FilterProductByName()
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
    public void GetTotalIncome(int sellerId)
    {
        List<Order> orders = _unitOfWork.Orders.GetAll();
        decimal totalIncome = 0;
        foreach (Order order in orders)
        {
            if (order.SellerId == sellerId && order.IsDeleted == false)
            {
                totalIncome += order.TotalPrice;
            }
        }
        Console.WriteLine($"Your total income is {totalIncome}");
    }
}
