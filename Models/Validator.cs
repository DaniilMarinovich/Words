using System;
using System.Globalization;

namespace Words.Models;

public class Validator
{

    public delegate void ErrorMessageHandler(string message, ConsoleColor color);
    private static ErrorMessageHandler _errorHandler;

    public static void SetErrorHandler(ErrorMessageHandler errorHandler)
    {
        _errorHandler = errorHandler;
    }

    public static bool IsWordLengthValid(string word)
    {
        if (word.Length >= Game.MinLength && word.Length <= Game.MaxLength)
        {
            return true;
        }
        _errorHandler?.Invoke("wordLengthError", ConsoleColor.DarkRed);

        return false;
    }

    public static bool IsWordLanguageConsistent(string word)
    {
        foreach (char letter in word)
        {
            if (IsLetterValidForLanguage(letter))
            {
                _errorHandler?.Invoke("invalidCharactersError", ConsoleColor.DarkRed);

                return false;
            }
        }

        return true;
    }

    public static bool IsLetterValidForLanguage(char letter)
    {
        if (CultureInfo.CurrentCulture.Name == "ru-RU")
        {
            return letter < 'а' || letter > 'я';
        }

        return letter < 'a' || letter > 'z';
    }

    public static bool IsInputWord(string word)
    {
        return word.Length < 2 || string.IsNullOrEmpty(word);
    }
}
