using System;
using System.Globalization;
using System.Resources;
using Words.Models;

namespace Words.Views;

public class GameConsoleView
{
    private ResourceManager resourceManager;
    private CultureInfo currentCulture = CultureInfo.CurrentCulture;

    public GameConsoleView()
    {
        resourceManager = new ResourceManager("Words.Resources.Messages", typeof(Words).Assembly);
    }

    public void SetApplicationCulture(CultureInfo language)
    {
        currentCulture = language;
        CultureInfo.CurrentCulture = language;
        CultureInfo.CurrentUICulture = language;
    }

    public string GetLocalizedMessage(string key)
    {
        return resourceManager.GetString(key, currentCulture);
    }

    public string? GetInput(string message = "")
    {
        if (!string.IsNullOrEmpty(message))
        {
            ShowMessage(message);
        }
        
        return Console.ReadLine();
    }

    public void ShowMessage(string message, ConsoleColor color = ConsoleColor.White)
    {
        Console.ForegroundColor = color;

        Console.WriteLine(GetLocalizedMessage(message));

        Console.ResetColor();
    }

    public void ShowMessage(string message, int parameter, ConsoleColor color = ConsoleColor.White)
    {
        Console.ForegroundColor = color;

        Console.WriteLine(string.Format(GetLocalizedMessage(message), parameter));

        Console.ResetColor();
    }

    public void ShowMessage(string message, string parameter, ConsoleColor color = ConsoleColor.White)
    {
        Console.ForegroundColor = color;

        Console.WriteLine(string.Format(GetLocalizedMessage(message), parameter));

        Console.ResetColor();
    }

    public void DisplayPlayerWords(Player player1, Player player2, int maxLength)
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;

        Console.WriteLine($"|{string.Format(GetLocalizedMessage("playerWordsHeader"), player1.Name).PadRight(maxLength)}|{string.Format(GetLocalizedMessage("playerWordsHeader"), player2.Name).PadRight(maxLength)}|");
        Console.WriteLine(new string('-', maxLength * 2 + 3));

        for (int i = 0; i < Math.Max(player1.Words.Count, player2.Words.Count); i++)
        {
            Console.WriteLine($"|{(i < player1.Words.Count ? player1.Words[i] : String.Empty).PadRight(maxLength)}|{(i < player2.Words.Count ? player2.Words[i] : String.Empty).PadRight(maxLength)}|");
        }

        Console.WriteLine($"|{string.Format(GetLocalizedMessage("wordCountMessage"), player1.Words.Count).PadRight(maxLength)}|{string.Format(GetLocalizedMessage("wordCountMessage"), player2.Words.Count).PadRight(maxLength)}|");
        Console.WriteLine(new string('-', maxLength * 2 + 3));

        Console.ResetColor();

        Console.WriteLine();
    }
}