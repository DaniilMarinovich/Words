using System;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Words.Models;

public class TurnTimer
{
    private Timer timer;

    public int TurnTime { get; set; }
    public bool IsTimerModeOn { get; set; }
    private int remainingTime { get; set; }
    public static bool isTimeUp { get; private set; }

    public delegate void ErrorMessageHandler(string message, ConsoleColor color);
    private readonly ErrorMessageHandler _errorHandler;

    public TurnTimer(ErrorMessageHandler errorHandler)
    {  
        _errorHandler = errorHandler; 
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

    public void StartTimer()
    {
        timer.Start();
    }
}
