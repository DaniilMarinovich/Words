using System.Collections.Generic;

namespace Words.Models;

public class Player
{
    public string Name {get; private set; }
    public List<string> Words { get; private set; }

    public Player(string name) 
    {
        Name = name;
        Words = [];
    }

    public void AddWord(string word)
    {
        Words.Add(word);
    }
}