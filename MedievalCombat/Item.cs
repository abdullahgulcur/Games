using System;
using System.Timers;

public class Item{

    /*
     * HeavyHeadGames
     * This class created by Abdullah Gulcur
     * 
     * When you add item to inventory instances of this class will be created
     * Every Item can stay up top 15 minutes, then instance will be destroyed automatically
     * 
     */

    string category;
    int index;
    int totalSeconds;

    private Timer aTimer;
    
    public Item(string category, int index, int totalSeconds)
    {
        this.category = category;
        this.index = index;
        this.totalSeconds = totalSeconds;

        SetTimer();
    }

    public int GetTotalSeconds()
    {
        return totalSeconds;
    }
    
    public string GetCategory()
    {
        return category;
    }

    public int GetIndex()
    {
        return index;
    }
    
    public override string ToString()
    {
        return "category: " + category + "index: " + index.ToString();
    }

    private void SetTimer()
    {
        aTimer = new Timer(1000);
        aTimer.Elapsed += OnTimedEvent;
        aTimer.AutoReset = true;
    }

    public void StopTimer()
    {
        aTimer.Stop();
    }

    public void StartTimer()
    {
        aTimer.Start();
    }

    private void OnTimedEvent(Object source, ElapsedEventArgs e)
    {
        totalSeconds++;
    }

}

/*
    private void SetTimer()
    {
        dt = DateTime.Now;
    }

public int GetTimeInSeconds()
{
    return (int)(DateTime.Now - dt).TotalSeconds;
}
*/
