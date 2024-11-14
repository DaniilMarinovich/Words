using System;
using System.Collections.Generic;

namespace Words;

public class Words
{
    private static Dictionary<char, int> wordLetters = new Dictionary<char, int>();
    private static string word;

    private static string player1;
    private static string player2;
    private static List<string> player1Words = new List<string>();
    private static List<string> player2Words = new List<string>();

    private const int MinLength = 8;
    private const int MaxLength = 30;

    public static void Main(string[] args)
    {
        CreateDictionary(ReadWord());
        PlayersNames();
        Process(player1);
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
        if (word.Length >= MinLength && word.Length <= MaxLength)
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

    private static void CreateDictionary(string word)
    {
        foreach (var letter in word)
        {
            if (wordLetters.ContainsKey(letter))
            {
                ++wordLetters[letter];
            }
            else
            {
                wordLetters.Add(letter, 1);
            }
        }
    }

    private static bool CheckWordFromOriginLetters(string word)
    {
        Dictionary<char, int> freeLetters = new Dictionary<char, int>(wordLetters);

        foreach (char letter in word)
        {
            if (freeLetters.ContainsKey(letter) && freeLetters[letter] > 0)
            {
                --freeLetters[letter];
            }
            else
            {
                ExceptionConsoleMessage("Слово содержит буквы не из исходного. Введите повторно.");

                return false;
            }
        }

        return true;
    }

    private static bool CheckWordIsUnic(string word)
    {
        if (!player1Words.Contains(word) && !player2Words.Contains(word))
        {
            return true;
        }
        else
        {
            ExceptionConsoleMessage("Слово уже было использовано одним из игроков. Введите повторно.");
            return false;
        }
    }

    private static void ExceptionConsoleMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    private static void PlayersNames()
    {
        Console.WriteLine("Введите имя первого игрока:");
        player1 = Console.ReadLine();

        Console.WriteLine("Введите имя второго игрока:");
        player2 = Console.ReadLine();
    }

    private static void AddWord(string player, string word)
    {
        if (player == player1)
        {
            player1Words.Add(word);
        }
        else if (player == player2)
        {
            player2Words.Add(word);
        }
        else
        {
            ExceptionConsoleMessage("Неверный игрок");
        }
    }

    private static void DisplayWords()
    {
        DisplayConsoleMessage($"|{$"Слова игрока {player1}".PadRight(MaxLength)}|{$"Слова игрока {player2}".PadRight(MaxLength)}|");
        DisplayConsoleMessage(new string('-', MaxLength*3 + 3));
        for (int i = 0; i < Math.Max(player1Words.Count, player2Words.Count); i++)
        {
            DisplayConsoleMessage($"|{(i < player1Words.Count ? player1Words[i] : String.Empty ).PadRight(MaxLength)}|{(i < player2Words.Count ? player2Words[i] : String.Empty).PadRight(MaxLength)}|");
        }
        DisplayConsoleMessage(new string('-', MaxLength*3 + 3));
        Console.WriteLine();
    }

    private static void DisplayConsoleMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;

        Console.WriteLine(message);

        Console.ResetColor();
    }

    private static void Process(string player)
    {
        Console.WriteLine($"\nХод игрока {player}");

        string playerWord;
        int playerAttempts = 3;

        do
        {
            playerWord = Console.ReadLine().ToLower();
            if (CheckWordFromOriginLetters(playerWord) && CheckWordIsUnic(playerWord))
            {
                AddWord(player, playerWord);
                DisplayConsoleMessage("Слово подходит!\n");
                Process(player == player1 ? player2 : player1);
            }
            else
            {
                ExceptionConsoleMessage($"Количество оставшихся попыток: {--playerAttempts}");
            }
        } while (playerAttempts > 0);

        Finish(player == player1 ? player2 : player1);
    }

    private static void Finish(string player)
    {
        DisplayConsoleMessage($"\nПобедитель {player}");
        Console.WriteLine("Все найденные слова:\n");
        DisplayWords();
        return;
    }
}