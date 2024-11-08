using System;
using System.Collections.Generic;

namespace Words;

public class Words
{

    private static string word;

    public static void Main(string[] args)
    {

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
}