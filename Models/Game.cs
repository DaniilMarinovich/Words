using System;
using System.Collections.Generic;
using System.Globalization;
using System.Timers;
using System.Xml.Linq;
using Words.Views;

namespace Words.Models;

public class Game
{
    public string OriginalWord { get; set; }
    private Dictionary<char, int> LetterFrequencies;

    public Player Player1 { get; private set; }
    public Player Player2 { get; private set; }

    public const int MinLength = 8;
    public const int MaxLength = 30;

    private Timer timer;
    public int TurnTime {get; set; }
    public bool IsTimerModeOn { get; set; }
    public int remainingTime { get; private set; }
    public bool isTimeUp { get; private set; }

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

    public void SetTurnTimer(int time)
    {
        IsTimerModeOn = true;
        TurnTime = time;
    }

    public void StartTurnTimer()
    {
        remainingTime = TurnTime;
        isTimeUp = false;
        timer = new Timer(1000);
        timer.Elapsed += TimerElapsed;
        timer.Start();
    }

    private void TimerElapsed(object sender, ElapsedEventArgs e)
    {
        remainingTime--;

        if (remainingTime <= 0)
        {
            isTimeUp = true;
            timer.Stop();
            _errorHandler?.Invoke("timeUpMessage", ConsoleColor.DarkYellow);
        }
    }

    public void StopTimer()
    {
        timer.Stop();
    }

    public bool IsWordLengthValid(string word)
    {
        if (word.Length >= Game.MinLength && word.Length <= Game.MaxLength)
        {
            return true;
        }
        _errorHandler?.Invoke("wordLengthError", ConsoleColor.DarkRed);

        return false;
    }

    public bool IsWordLanguageConsistent(string word)
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

    public bool ValidateWord(string word)
    {
        return !IsWordLengthValid(word) || !IsWordLanguageConsistent(word);
    }

    public bool IsLetterValidForLanguage(char letter)
    {
        // Search, how do this as OOP
        if (CultureInfo.CurrentCulture.Name == "ru-RU")
        {
            return letter < 'а' || letter > 'я';
        }

        return letter < 'a' || letter > 'z';
    }

    public bool IsInputWord(string word)
    {
        return word.Length < 2 || string.IsNullOrEmpty(word);
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
        return attempts > 0 && !isTimeUp;
    }
}