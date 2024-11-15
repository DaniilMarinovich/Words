using System;
using System.Collections.Generic;
using System.Timers;

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

    private static Timer timer;
    private static bool isTimerModeOn = false;
    private static int remainingTime;
    private static bool isTimeUp = false;
    private static int turnTime;

    public static void Main(string[] args)
    {
        Menu();
        CreateDictionary(ReadWord());
        InputPlayersNames();
        GameProcess(player1);
    }

    private static void Menu() 
    {
        while (true)
        {
            Console.WriteLine("\nМеню:" +
                "\n\t1. Выбор языка игры." +
                "\n\t2. Выбор режима игры." +
                "\n\t3. Начать игру.");

            switch(Console.ReadLine())
            {
                case "1":
                    break;
                case "2":
                    SelectTimer();
                    break;
                case "3":
                    return;
                default:
                    ExceptionConsoleMessage("Неверный выбор. Повторите выбор.");
                    break;
            }
        }
    }

    private static void SelectTimer()
    {
        Console.WriteLine("Добро пожаловать в меню выбора таймера:" +
            "\n\t1. 2 минуты." +
            "\n\t2. 1 минута." +
            "\n\t3. 30 секунд." +
            "\n\t4. 10 секунд." +
            "\n\t5. Продолжить без таймера.");

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
                    ExceptionConsoleMessage("Неверный выбор. Повторите выбор.");
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
                ExceptionConsoleMessage("Слово содержит буквы не из исходного. Введите повторно.");

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

    private static void InputPlayersNames()
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

    private static void WordsOutput()
    {
        MarkupConsoleMessage($"|{$"Слова игрока {player1}".PadRight(MaxLength)}|{$"Слова игрока {player2}".PadRight(MaxLength)}|");
        MarkupConsoleMessage(new string('-', MaxLength*2 + 3));
        for (int i = 0; i < Math.Max(player1Words.Count, player2Words.Count); i++)
        {
            MarkupConsoleMessage($"|{(i < player1Words.Count ? player1Words[i] : String.Empty ).PadRight(MaxLength)}|{(i < player2Words.Count ? player2Words[i] : String.Empty).PadRight(MaxLength)}|");
        }
        MarkupConsoleMessage($"|{$"Количество слов : {player1Words.Count}".PadRight(MaxLength)}|{$"Количество слов : {player2Words.Count}".PadRight(MaxLength)}|");
        MarkupConsoleMessage(new string('-', MaxLength*2 + 3));
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
                Console.WriteLine("\nВремя вышло! Ход игрока завершен.");
            }
        }

    private static void GameProcess(string player)
    {
        Console.WriteLine($"\nХод игрока {player}");

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
                    MarkupConsoleMessage("Слово подходит!\n");
                    if (isTimerModeOn)
                    {
                        timer.Stop();
                    }
                    GameProcess(player == player1 ? player2 : player1);
                }
                else
                {
                    ExceptionConsoleMessage($"Количество оставшихся попыток: {--playerAttempts}");
                }
            }
        }

        if (isTimerModeOn)
        {
            if(isTimeUp)
            {
                GameProcessFinish(player == player1 ? player2 : player1);
            }
        }
        else
        {
            GameProcessFinish(player == player1 ? player2 : player1);
        }
    }

    private static void GameProcessFinish(string player)
    {
        MarkupConsoleMessage($"\nПобедитель {player}");
        Console.WriteLine("Все найденные слова:\n");
        WordsOutput();
        return;
    }
}