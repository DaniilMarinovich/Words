using System;
using System.Collections.Generic;

namespace Words.Models;

public class Game
{
    public string OriginalWord { get; set; }
    private Dictionary<char, int> LetterFrequencies;

    public Player Player1 { get; private set; }
    public Player Player2 { get; private set; }

    public const int MinLength = 8;
    public const int MaxLength = 30;

    public delegate void ErrorMessageHandler(string message, ConsoleColor color);
    private readonly ErrorMessageHandler _errorHandler;

    public Game(Player player1, Player player2, ErrorMessageHandler errorHandler)
    {
        Player1 = player1;
        Player2 = player2;
        _errorHandler = errorHandler;
        LetterFrequencies =  new Dictionary<char, int>();
    }

    public void CreateLetterFrequencyDictionary(string word)
    {
        foreach (var letter in word)
        {
            if (LetterFrequencies.ContainsKey(letter))
            {
                ++LetterFrequencies[letter];
            }
            else
            {
                LetterFrequencies.Add(letter, 1);
            }
        }
    }

    public bool IsWordCommand(string word)
    {
        return word.StartsWith("/");
    }

    public bool ValidateWord(string word)
    {
        return !Validator.IsWordLengthValid(word) || !Validator.IsWordLanguageConsistent(word);
    }

    public bool CanWordBeConstructed(string word)
    {
        Dictionary<char, int> freeLetters = new Dictionary<char, int>(LetterFrequencies);

        foreach (char letter in word)
        {
            if (freeLetters.ContainsKey(letter) && freeLetters[letter] > 0)
            {
                --freeLetters[letter];
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    public bool IsWordUnique(string word)
    {
        if (!Player1.Words.Contains(word) && !Player2.Words.Contains(word))
        {
            return true;
        }
        else
        {
            _errorHandler?.Invoke("wordUsedError", ConsoleColor.DarkRed);
            return false;
        }
    }

    public bool IsRoundEnd(int attempts)
    {
        return attempts > 0 && !TurnTimer.isTimeUp;
    }
}