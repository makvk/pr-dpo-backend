using Microsoft.AspNetCore.Identity;

namespace StudentPortal.Diagnostics.Services;

public class GenerateService: IGenerateService
{
    private const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
    private const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Digits = "0123456789";
    private const string SpecialChars = "!@#$%^&*()_+-=[]{};:,.<>?";
    private readonly Random _random = new();
    public string GenPassword()
    {
        var password = string.Empty; 
        var availableChars = Lowercase + Uppercase + Digits + SpecialChars;
        password = new string(Enumerable.Repeat(availableChars, 10)
            .Select(s => s[_random.Next(s.Length)])
            .ToArray());
        return password;
    }
}