using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Timers;

namespace Words;

public class Words
{
    private static ResourceManager resourceManager;
    private static CultureInfo currentCulture = CultureInfo.CurrentCulture;

    private static Dictionary<char, int> wordLetters = new Dictionary<char, int>();
    private static string word;

    private const int MinLength = 8;
    private const int MaxLength = 30;

    private static string player1;
    private static string player2;
    private static List<string> player1Words = new List<string>();
    private static List<string> player2Words = new List<string>();

    private static Timer timer;
    private static bool isTimerModeOn = false;
    private static int remainingTime;
    private static bool isTimeUp = false;
    private static int turnTime;

    public static void Main(string[] args)
    {
        resourceManager = new ResourceManager("Words.Resources.Messages", typeof(Words).Assembly);
        ShowMainMenu();
        CreateLetterFrequencyDictionary(ReadAndValidateWord());
        InputPlayersNames();
        ExecutePlayerTurn(player1);
    }

    private static void ShowMainMenu()
    {
        while (true)
        {
            Console.WriteLine(GetLocalizedMessage("menuHead"));

            switch(Console.ReadLine())
            {
                case "1":
                    SelectLanguage();
                    break;
                case "2":
                    SelectTimer();
                    break;
                case "3":
                    return;
                default:
                    ShowErrorMessage(GetLocalizedMessage("invalidChoiceMessage"));
                    break;
            }
        }
    }

    private static void SelectLanguage()
    {
        Console.WriteLine(GetLocalizedMessage("languageMenuHead"));

        while (!isTimerModeOn)
        {
            switch (Console.ReadLine())
            {
                case "1":
                    SetApplicationCulture(new CultureInfo("ru-RU"));
                    return;
                case "2":
                    SetApplicationCulture(new CultureInfo("en-US"));
                    return;
                case "3":
                    return;
                default:
                    ShowErrorMessage(GetLocalizedMessage("invalidChoiceMessage"));
                    break;
            }
        }
    }

    private static void SetApplicationCulture(CultureInfo language)
    {
        currentCulture = language;
        CultureInfo.CurrentCulture = language;
        CultureInfo.CurrentUICulture = language;
    }

    private static void SelectTimer()
    {
        Console.WriteLine(GetLocalizedMessage("timerMenuHead"));

        while (!isTimerModeOn)
        {
            switch (Console.ReadLine())
            {
                case "1":
                    SetTurnTimer(120);
                    return;
                case "2":
                    SetTurnTimer(60);
                    return;
                case "3":
                    SetTurnTimer(30);
                    return;
                case "4":
                    SetTurnTimer(10);
                    return;
                case "5":
                    return;
                default:
                    ShowErrorMessage(GetLocalizedMessage("invalidChoiceMessage"));
                    break;
            }
        }
    }

    private static void SetTurnTimer(int time)
    {
        isTimerModeOn = true;
        turnTime = time;
    }

    private static string ReadWord()
    {
        do
        {
            Console.WriteLine(GetLocalizedMessage("enterWordPrompt"));
            word = Console.ReadLine().ToLower();
        } while (ValidateWord(word));

        return word;
    }

    private static bool IsWordLengthValid(string word)
    {
        if (word.Length >= MinLength && word.Length <= MaxLength)
        {
            return true;
        }
        ShowErrorMessage(GetLocalizedMessage("wordLengthError"));

        return false;
    }

    private static bool IsWordLanguageConsistent(string word)
    {
        foreach (char letter in word)
        {
            if (IsLetterValidForLanguage(letter))
            {
                ShowErrorMessage(GetLocalizedMessage("invalidCharactersError"));

                return false;
            }
        }

        return true;
    }

    private static bool ValidateWord(string word)
    {
        return !IsWordLengthValid(word) || !IsWordLanguageConsistent(word);
    }

    private static bool IsLetterValidForLanguage(char letter)
    {
        // Search, how do this as OOP
        if (currentCulture.Name == "ru-RU")
        {
            return letter < 'а' || letter > 'я';
        }

        return letter < 'a' || letter > 'z';
    }

    private static void CreateLetterFrequencyDictionary(string word)
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

    private static bool CanWordBeConstructed(string word)
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
                ShowErrorMessage(GetLocalizedMessage("lettersFromOriginalError"));

                return false;
            }
        }

        return true;
    }

    private static bool IsInputWord(string word)
    {
        return word.Length >= 2 || String.IsNullOrEmpty(word);
    }

    private static bool IsWordUnique(string word)
    {
        if (!player1Words.Contains(word) && !player2Words.Contains(word))
        {
            return true;
        }
        else
        {
            ShowErrorMessage(GetLocalizedMessage("wordUsedError"));
            return false;
        }
    }

    private static void ShowErrorMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    private static void InputPlayersNames()
    {
        Console.WriteLine(GetLocalizedMessage("firstPlayerNamePrompt"));
        player1 = Console.ReadLine();

        Console.WriteLine(GetLocalizedMessage("secondPlayerNamePrompt"));
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
            ShowErrorMessage(GetLocalizedMessage("invalidPlayerError"));
        }
    }

    private static void DisplayPlayerWords()
    {
        HighlightConsoleMessage($"|{string.Format(GetLocalizedMessage("playerWordsHeader"), player1).PadRight(MaxLength)}|{string.Format(GetLocalizedMessage("playerWordsHeader"), player2).PadRight(MaxLength)}|");
        HighlightConsoleMessage(new string('-', MaxLength * 2 + 3));

        for (int i = 0; i < Math.Max(player1Words.Count, player2Words.Count); i++)
        {
            HighlightConsoleMessage($"|{(i < player1Words.Count ? player1Words[i] : String.Empty).PadRight(MaxLength)}|{(i < player2Words.Count ? player2Words[i] : String.Empty).PadRight(MaxLength)}|");
        }

        HighlightConsoleMessage($"|{string.Format(GetLocalizedMessage("wordCountMessage"), player1Words.Count).PadRight(MaxLength)}|{string.Format(GetLocalizedMessage("wordCountMessage"), player2Words.Count).PadRight(MaxLength)}|");
        HighlightConsoleMessage(new string('-', MaxLength * 2 + 3));
        Console.WriteLine();
    }

    private static void HighlightConsoleMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;

        Console.WriteLine(message);

        Console.ResetColor();
    }

    private static void StartTurnTimer()
    {
        remainingTime = turnTime;
        isTimeUp = false;
        timer = new Timer(1000);
        timer.Elapsed += TimerElapsed;
        timer.Start();
    }

    private static void TimerElapsed(object sender, ElapsedEventArgs e)
    {
        remainingTime--;

        if (remainingTime <= 0)
        {
            isTimeUp = true;
            timer.Stop();
            TimerConsoleMessage(GetLocalizedMessage("timeUpMessage"));
        }
    }

    private static void TimerConsoleMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    private static void ExecutePlayerTurn(string player)
    {
        Console.WriteLine($"\n{string.Format(GetLocalizedMessage("playerTurn"), player)}");

        string playerWord;
        int playerAttempts = 3;
        if(isTimerModeOn)
        {
            StartTurnTimer();
        }

        while (playerAttempts > 0 && !isTimeUp)
        {
            if (Console.KeyAvailable)
            {
                playerWord = Console.ReadLine().ToLower();
                if (IsInputWord(playerWord))
                { 
                    ShowErrorMessage(GetLocalizedMessage("emptinessAbsenceWordError")); 
                }
                else if (CanWordBeConstructed(playerWord) && IsWordUnique(playerWord))
                {
                    AddWord(player, playerWord);
                    HighlightConsoleMessage(GetLocalizedMessage("rightWord"));
                    if (isTimerModeOn)
                    {
                        timer.Stop();
                    }
                    ExecutePlayerTurn(player == player1 ? player2 : player1);
                }
                else
                {
                    ShowErrorMessage(string.Format(GetLocalizedMessage("remainingAttemptsMessage"), --playerAttempts));
                }
            }
        }

        EndGameRound(player == player1 ? player2 : player1);
    }

    private static void EndGameRound(string player)
    {
        HighlightConsoleMessage(string.Format(GetLocalizedMessage("roundWinnerMessage"), player));
        Console.WriteLine(GetLocalizedMessage("allWords"));
        DisplayPlayerWords();
        return;
    }

    private static string GetLocalizedMessage(string key)
    {
        return resourceManager.GetString(key, currentCulture);
    }
}