using System.Collections.Generic;
using System.Dynamic;

namespace Words.Models;

public class Game
{
    public string OriginalWord { get; private set; }
    public Dictionary<char, int> LetterFrequencies { get; private set; }

    public Player Player1 { get; private set; }
    public Player Player2 { get; private set; }

    public int TurnTime {get; set;}
    public bool IsTimerModeOn { get; set; }

    public Game(string word, Player player1, Player player2)
    {
        OriginalWord = word;
        Player1 = player1;
        Player2 = player2;
        LetterFrequencies = CreateLetterFrequencyDictionary(word);
    }

    private Dictionary<char, int> CreateLetterFrequencyDictionary(string word)
    {
        var frequencies = new Dictionary<char, int>();
        foreach (var letter in word)
        {
            if (frequencies.ContainsKey(letter))
            {
                ++frequencies[letter];
            }
            else
            {
                frequencies.Add(letter, 1);
            }
        }

        return frequencies;
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

}
