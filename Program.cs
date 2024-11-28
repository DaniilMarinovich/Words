using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Timers;
using Words.Controllers;
using Words.Models;
using Words.Views;

namespace Words;

public class Words
{
    public static void Main(string[] args)
    {
        var view = new GameView();

        var player1 = new Player(view.GetInput("firstPlayerNamePrompt"));
        var player2 = new Player(view.GetInput("secondPlayerNamePrompt"));

        var game = new Game(player1, player2, view.ShowMessage);
        var controller = new GameController(game, view);

        controller.StartGame();
    }
}