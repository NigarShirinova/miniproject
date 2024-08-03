using Core.Constants;
using Core.Entities;
using Data.Context;
using Microsoft.AspNetCore.Identity;

namespace Data;
public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.EnsureCreated();
        if (context.Admins.Any())
        {
            return;
        }
        string password = "admin123";
        PasswordHasher<Admin> passwordHasher = new PasswordHasher<Admin>();
        Admin admin = new Admin()
        {
            Name = "admin",
            Surname = "admin",
            CreatedDate = DateTime.Now,
            Email = "admin123@gmail.com"
        };
        admin.Password = passwordHasher.HashPassword(admin, password);
        context.Admins.Add(admin);
        try
        {
            context.SaveChanges();
        }
        catch (Exception ex)
        {
            Messages.ErrorOccuredMessage();
        }
    }
}
