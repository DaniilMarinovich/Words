using System;
using Words.Controllers;
using Words.Models;
using Words.Views;

namespace Words;

public class Words
{
    public static void Main(string[] args)
    {
        var gameView = new GameConsoleView();
        var scoreView = new ScoreView("score.json");

        var player1 = new Player(gameView.GetInput("firstPlayerNamePrompt"));
        var player2 = new Player(gameView.GetInput("secondPlayerNamePrompt"));

        var turnTimer = new TurnTimer(gameView.ShowMessage);
        Validator.SetErrorHandler(gameView.ShowMessage);

        var game = new Game(player1, player2, gameView.ShowMessage);
        var controller = new GameController(game, gameView, scoreView, turnTimer);

        AppDomain.CurrentDomain.ProcessExit += controller.OnApplicationExit;

        controller.StartGame();
    }
}