namespace Core.Constants;
public class Messages
{
    public static void ErrorOccuredMessage() { Console.ForegroundColor = ConsoleColor.Red;  Console.WriteLine($"Error occured"); }
    public static void InputMessage(string message) { Console.ForegroundColor = ConsoleColor.White; Console.WriteLine($"Please, input {message}"); }
    public static void InvalidInputMessage(string message) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"Invalid {message} input, please try again"); }
    public static void SuccessMessage() { Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("Operation successfully done!"); }
    public static void PasswordFormMessage() { Console.ForegroundColor = ConsoleColor.White; Console.WriteLine("Password must contain a minimum of 1 numeric character [0-9],1 uppercase, 1 lowercase letter and 1 special character "); }
    public static void NotFoundMessage(string message) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"{message} not found"); }
}
