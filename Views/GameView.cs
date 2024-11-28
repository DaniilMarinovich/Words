using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Words.Models;

namespace Words.Views;

public class GameView
{
    private static ResourceManager resourceManager;
    private static CultureInfo currentCulture = CultureInfo.CurrentCulture;

    public string GetLocalizedMessage(string key)
    {
        return resourceManager.GetString(key, currentCulture);
    }

    public string GetInput()
    {
        return Console.ReadLine();
    }

    public void ShowErrorMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public void HighlightConsoleMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;

        Console.WriteLine(message);

        Console.ResetColor();
    }

    public void DisplayPlayerWords(Player player1, Player player2, int maxLength)
    {
        HighlightConsoleMessage($"|{string.Format(GetLocalizedMessage("playerWordsHeader"), player1).PadRight(maxLength)}|{string.Format(GetLocalizedMessage("playerWordsHeader"), player2).PadRight(maxLength)}|");
        HighlightConsoleMessage(new string('-', maxLength * 2 + 3));

        for (int i = 0; i < Math.Max(player1.Words.Count, player2.Words.Count); i++)
        {
            HighlightConsoleMessage($"|{(i < player1.Words.Count ? player1.Words[i] : String.Empty).PadRight(maxLength)}|{(i < player2.Words.Count ? player2.Words[i] : String.Empty).PadRight(maxLength)}|");
        }

        HighlightConsoleMessage($"|{string.Format(GetLocalizedMessage("wordCountMessage"), player1.Words.Count).PadRight(maxLength)}|{string.Format(GetLocalizedMessage("wordCountMessage"), player2.Words.Count).PadRight(maxLength)}|");
        HighlightConsoleMessage(new string('-', maxLength * 2 + 3));
        Console.WriteLine();
    }
}
