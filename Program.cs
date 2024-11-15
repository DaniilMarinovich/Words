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
        Menu();
        CreateDictionary(ReadWord());
        InputPlayersNames();
        GameProcess(player1);
    }

    private static void Menu()
    {
        while (true)
        {
            Console.WriteLine(GetMessage("menuHead"));

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
                    ExceptionConsoleMessage(GetMessage("invalidChoiceMessage"));
                    break;
            }
        }
    }

    private static void SelectLanguage()
    {
        Console.WriteLine(GetMessage("languageMenuHead"));

        while (!isTimerModeOn)
        {
            switch (Console.ReadLine())
            {
                case "1":
                    SetLanguage(new CultureInfo("ru-RU"));
                    return;
                case "2":
                    SetLanguage(new CultureInfo("en-US"));
                    return;
                case "3":
                    return;
                default:
                    ExceptionConsoleMessage(GetMessage("invalidChoiceMessage"));
                    break;
            }
        }
    }

    private static void SetLanguage(CultureInfo language)
    {
        currentCulture = language;
        CultureInfo.CurrentCulture = language;
        CultureInfo.CurrentUICulture = language;
    }

    private static void SelectTimer()
    {
        Console.WriteLine(GetMessage("timerMenuHead"));

        while (!isTimerModeOn)
        {
            switch (Console.ReadLine())
            {
                case "1":
                    SetTimer(120);
                    return;
                case "2":
                    SetTimer(60);
                    return;
                case "3":
                    SetTimer(30);
                    return;
                case "4":
                    SetTimer(10);
                    return;
                case "5":
                    return;
                default:
                    ExceptionConsoleMessage(GetMessage("invalidChoiceMessage"));
                    break;
            }
        }
    }

    private static void SetTimer(int time)
    {
        isTimerModeOn = true;
        turnTime = time;
    }

    private static string ReadWord()
    {
        do
        {
            Console.WriteLine(GetMessage("enterWordPrompt"));
            word = Console.ReadLine().ToLower();
        } while (!CheckWordLength(word) || !CheckWordLanguageOfLetters(word));

        return word;
    }

    private static bool CheckWordLength(string word)
    {
        if (word.Length >= MinLength && word.Length <= MaxLength)
        {
            return true;
        }
        ExceptionConsoleMessage(GetMessage("wordLengthError"));

        return false;
    }

    private static bool CheckWordLanguageOfLetters(string word)
    {
        foreach (char letter in word)
        {
            if (CheckLetter(letter))
            {
                ExceptionConsoleMessage(GetMessage("invalidCharactersError"));

                return false;
            }
        }

        return true;
    }

    private static bool CheckLetter(char letter)
    {
        // Search, how do this as OOP
        if (currentCulture.Name == "ru-RU")
        {
            return letter < 'а' || letter > 'я';
        }

        return letter < 'a' || letter > 'z';
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

    private static bool CheckWordFromOriginalLetters(string word)
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
                ExceptionConsoleMessage(GetMessage("lettersFromOriginalError"));

                return false;
            }
        }

        return true;
    }

    private static bool CheckWordUniqueness(string word)
    {
        if (!player1Words.Contains(word) && !player2Words.Contains(word))
        {
            return true;
        }
        else
        {
            ExceptionConsoleMessage(GetMessage("wordUsedError"));
            return false;
        }
    }

    private static void ExceptionConsoleMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    private static void InputPlayersNames()
    {
        Console.WriteLine(GetMessage("firstPlayerNamePrompt"));
        player1 = Console.ReadLine();

        Console.WriteLine(GetMessage("secondPlayerNamePrompt"));
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
            ExceptionConsoleMessage(GetMessage("invalidPlayerError"));
        }
    }

    private static void WordsOutput()
    {
        MarkupConsoleMessage($"|{string.Format(GetMessage("playerWordsHeader"), player1).PadRight(MaxLength)}|{string.Format(GetMessage("playerWordsHeader"), player2).PadRight(MaxLength)}|");
        MarkupConsoleMessage(new string('-', MaxLength * 2 + 3));

        for (int i = 0; i < Math.Max(player1Words.Count, player2Words.Count); i++)
        {
            MarkupConsoleMessage($"|{(i < player1Words.Count ? player1Words[i] : String.Empty).PadRight(MaxLength)}|{(i < player2Words.Count ? player2Words[i] : String.Empty).PadRight(MaxLength)}|");
        }

        MarkupConsoleMessage($"|{string.Format(GetMessage("wordCountMessage"), player1Words.Count).PadRight(MaxLength)}|{string.Format(GetMessage("wordCountMessage"), player2Words.Count).PadRight(MaxLength)}|");
        MarkupConsoleMessage(new string('-', MaxLength * 2 + 3));
        Console.WriteLine();
    }

    private static void MarkupConsoleMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;

        Console.WriteLine(message);

        Console.ResetColor();
    }

    private static void StartTimer()
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
            TimerConsoleMessage(GetMessage("timeUpMessage"));
        }
    }

    private static void TimerConsoleMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    private static void GameProcess(string player)
    {
        Console.WriteLine($"\n{string.Format(GetMessage("playerTurn"), player)}");

        string playerWord;
        int playerAttempts = 3;
        if(isTimerModeOn)
        {
            StartTimer();
        }

        while (playerAttempts > 0 && !isTimeUp)
        {
            if (Console.KeyAvailable)
            {
                playerWord = Console.ReadLine().ToLower();
                if (CheckWordFromOriginalLetters(playerWord) && CheckWordUniqueness(playerWord))
                {
                    AddWord(player, playerWord);
                    MarkupConsoleMessage(GetMessage("rightWord"));
                    if (isTimerModeOn)
                    {
                        timer.Stop();
                    }
                    GameProcess(player == player1 ? player2 : player1);
                }
                else
                {
                    ExceptionConsoleMessage(string.Format(GetMessage("remainingAttemptsMessage"), --playerAttempts));
                }
            }
        }

        GameProcessFinish(player == player1 ? player2 : player1);
    }

    private static void GameProcessFinish(string player)
    {
        MarkupConsoleMessage(string.Format(GetMessage("roundWinnerMessage"), player));
        Console.WriteLine(GetMessage("allWords"));
        WordsOutput();
        return;
    }

    private static string GetMessage(string key)
    {
        return resourceManager.GetString(key, currentCulture);
    }
}