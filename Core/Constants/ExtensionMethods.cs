using System.Text.RegularExpressions;

namespace Core.Constants;
public static class ExtensionMethods
{
    public static bool IsValidEmail(this string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        try
        {
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            var regex = new Regex(emailPattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
    public static bool IsValidPassword(this string password, int minLength = 8, int maxLength = 100)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return false;
        }
        if (password.Length < minLength || password.Length > maxLength)
        {
            return false;
        }
        if (!Regex.IsMatch(password, @"[A-Z]"))
        {
            return false;
        }
        if (!Regex.IsMatch(password, @"[a-z]"))
        {
            return false;
        }
        if (!Regex.IsMatch(password, @"\d"))
        {
            return false;
        }
        if (!Regex.IsMatch(password, @"[\W_]"))
        {
            return false;
        }

        return true;
    }

    public static bool IsValidPhoneNumber(this string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            return false;
        }
        var phonePattern = @"^\+994[0-9]{9}$";
        var regex = new Regex(phonePattern, RegexOptions.IgnoreCase);

        return regex.IsMatch(phoneNumber);
    }

    public static bool IsValidPIN(this string pin)
    {
        if (string.IsNullOrWhiteSpace(pin))
        {
            return false;
        }
        var finPattern = @"^[A-Z0-9]{7}$";
        var regex = new Regex(finPattern, RegexOptions.IgnoreCase);

        return regex.IsMatch(pin);
    }
}
