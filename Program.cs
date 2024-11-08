using System;
using System.Collections.Generic;

namespace Words;

public class Words
{
    private static Dictionary<string, int> wordLetters;
    private static string word;

    private static string Player1;
    private static string Player2;
    private static List<string> Player1Words = new List<string>();
    private static List<string> Player2Words = new List<string>();

    public static void Main(string[] args)
    {
        ReadWord();
        PlayersNames();

        for (int i = 0; i < 10; i++)
        {
            AddWord(Player1, "ckjad");
            AddWord(Player2, "ckjad");
        }

        DisplayWords();

        Console.WriteLine(word);
    }

    private static string ReadWord()
    {
        do
        {
            Console.WriteLine("\nВведите изначальное слово:");
            word = Console.ReadLine().ToLower();
        } while (!CheckWordLength(word) || !CheckWordRussianLetters(word));

        return word;
    }

    private static bool CheckWordLength(string word)
    {
        if (word.Length >= 8 && word.Length <= 30)
        {
            return true;
        }

        ExceptionConsoleMessage("Слово имеет неверную длинну. Введите повторно.");
        return false;
    }

    private static bool CheckWordRussianLetters(string word)
    {
        foreach (char letter in word)
        {
            if ((letter < 'а') || (letter > 'я'))
            {
                ExceptionConsoleMessage("Слово содержит неверные символы. Введите повторно.");
                return false;
            }
        }

        return true;
    }

    private static void ExceptionConsoleMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.BackgroundColor = ConsoleColor.White;

        Console.WriteLine(message);

        Console.ResetColor();
    }

    private static void PlayersNames()
    {
        Console.WriteLine("Введите имя первого игрока:");
        Player1 = Console.ReadLine();

        Console.WriteLine("Введите имя второго игрока:");
        Player2 = Console.ReadLine();
    }

    private static void AddWord(string player, string word)
    {
        if (player == Player1)
        {
            Player1Words.Add(word);
        }
        else if (player == Player2)
        {
            Player2Words.Add(word);
        }
        else
        {
            ExceptionConsoleMessage("Неверный игрок");
        }
    }

    private static void DisplayWords()
    {
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine($"|{"Слова первого игрока".PadRight(30)}|{"Слова второго игрока".PadRight(30)}|");
        Console.WriteLine(new string('-', 63));
        for (int i = 0; i < Math.Max(Player1Words.Count, Player2Words.Count); i++)
        {

            Console.WriteLine($"|{(i < Player1Words.Count ? Player1Words[i] : String.Empty ).PadRight(30)}|{(i < Player2Words.Count ? Player1Words[i] : String.Empty).PadRight(30)}|");
        }
        Console.WriteLine(new string('-', 63));
        Console.WriteLine();
    }
}