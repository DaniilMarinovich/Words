﻿using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Timers;
using System;
using Words.Models;
using Words.Views;
using System.Threading;
using System.Numerics;

namespace Words.Controllers;

public class GameController
{
    private readonly Game _game;
    private readonly GameView _gameView;
    private readonly ScoreView _scoreView;

    private Player currentPlayer;

    public GameController(Game game, GameView gameView, ScoreView scoreView)
    {
        _game = game;
        _gameView = gameView;
        _scoreView = scoreView;
    }

    public void StartGame()
    {
        ShowMainMenu();
        ReadWord();
        ExecutePlayerTurn(_game.Player1);
    }

    private void ShowMainMenu()
    {
        while (true)
        {
            _gameView.ShowMessage("menuHead");

            switch (_gameView.GetInput())
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
                    _gameView.ShowMessage("invalidChoiceMessage", ConsoleColor.DarkRed);
                    break;
            }
        }
    }

    private void SelectLanguage()
    {
        _gameView.ShowMessage("languageMenuHead");

        while (true)
        {
            switch (_gameView.GetInput())
            {
                case "1":
                    _gameView.SetApplicationCulture(new CultureInfo("ru-RU"));
                    return;
                case "2":
                    _gameView.SetApplicationCulture(new CultureInfo("en-US"));
                    return;
                case "3":
                    return;
                default:
                    _gameView.ShowMessage("invalidChoiceMessage", ConsoleColor.DarkRed);
                    break;
            }
        }
    }

    private void SelectTimer()
    {
        _gameView.ShowMessage("timerMenuHead");  

        while (true)
        {
            switch (_gameView.GetInput())
            {
                case "1":
                    _game.SetTurnTimer(120);
                    return;
                case "2":
                    _game.SetTurnTimer(60);
                    return;
                case "3":
                    _game.SetTurnTimer(30);
                    return;
                case "4":
                    _game.SetTurnTimer(10);
                    return;
                case "0":
                    return;
                default:
                    _gameView.ShowMessage("invalidChoiceMessage", ConsoleColor.DarkRed);
                    break;
            }
        }
    }

    private void ReadWord()
    {
        do
        {
            _game.OriginalWord = _gameView.GetInput("enterWordPrompt").ToLower();
        } while (_game.ValidateWord(_game.OriginalWord));

        _game.CreateLetterFrequencyDictionary(_game.OriginalWord);
    }

    private void ExecutePlayerTurn(Player player)
    {
        _gameView.ShowMessage("playerTurn", player.Name);
        currentPlayer = player;

        string playerWord;
        int playerAttempts = 3;
        if (_game.IsTimerModeOn)
        {
            _game.StartTurnTimer();
        }

        while (_game.IsRoundEnd(playerAttempts))
        {
            if (Console.KeyAvailable)
            {
                playerWord = _gameView.GetInput().ToLower();
                if (_game.IsWordCommand(playerWord))
                {
                    switch (playerWord) 
                    {
                        case "/show-words":
                            _gameView.DisplayPlayerWords(_game.Player1, _game.Player2, Game.MaxLength);
                            break;
                        case "/score":
                            _gameView.ShowMessage("currentPlayersScore", $"{_game.Player1.Name}: {_scoreView.GetScore(_game.Player1.Name)} - {_game.Player2.Name}: {_scoreView.GetScore(_game.Player2.Name)}", ConsoleColor.DarkGreen);
                            break;
                        case "/total-score":
                            _gameView.ShowMessage("allScores", _scoreView.GetAllScores(), ConsoleColor.DarkGreen);
                            break;
                    }

                }
                else if (_game.IsInputWord(playerWord))
                {
                    _gameView.ShowMessage("emptinessAbsenceWordError");
                }
                else if (_game.CanWordBeConstructed(playerWord) && _game.IsWordUnique(playerWord))
                {
                    player.AddWord(playerWord);
                    _gameView.ShowMessage("rightWord");
                    if (_game.IsTimerModeOn)
                    {
                        _game.StopTimer();
                    }

                    if (_game.IsRoundEnd(playerAttempts))
                    {
                        ExecutePlayerTurn(player == _game.Player1? _game.Player2 : _game.Player1);
                        return;
                    }
                }
                else
                {
                    _gameView.ShowMessage("remainingAttemptsMessage", --playerAttempts, ConsoleColor.DarkRed);
                }
            }
        }

        EndGameRound(player == _game.Player1 ? _game.Player2 : _game.Player1);
    }

    public void OnApplicationExit(object? sender, EventArgs e)
    {
        if (currentPlayer != null)
        {
            _scoreView.AddWin((currentPlayer == _game.Player1 ? _game.Player2 : _game.Player1).Name);
        }
    }

    private void EndGameRound(Player player)
    {
        _gameView.ShowMessage("roundWinnerMessage", player.Name, ConsoleColor.DarkGreen);
        _gameView.ShowMessage("allWords");
        _scoreView.AddWin(player.Name);
        _gameView.DisplayPlayerWords(_game.Player1, _game.Player2, Game.MaxLength);
        currentPlayer = null;
        return;
    }
}