using System;
using System.Collections.Generic;

public class Beatmap
{
    private List<EventHandler<BeatEventArgs>> eventHandlers;
    private List<BeatTrigger> beatEvents;

    public Beatmap()
    {

    }

    public void Step(float from, float to)
    {
        int start = beatEvents.BinarySearch(new BeatTrigger(from));
        int end = beatEvents.BinarySearch(new BeatTrigger(to));

        if (start < 0) start = ~start;
        if (end < 0) end = ~end;

        while (start > 0 && beatEvents[start - 1].Timestamp == from) start--;
        while (end < beatEvents.Count - 1 && beatEvents[end + 1].Timestamp == to) end++;

        foreach (BeatTrigger trigger in beatEvents.GetRange(start, end))
        {
            trigger.Trigger(to);
        }
    }
}

public class BeatEventArgs : EventArgs
{
    public float SecondsElapsed { get; set; }
    public BeatEventArgs(float secondsElapsed)
    {
        SecondsElapsed = secondsElapsed;
    }
}

public struct BeatTrigger : IComparable<BeatTrigger>
{
    public float Timestamp;

    public BeatTrigger(float timestamp)
    {
        Timestamp = timestamp;
    }

    protected virtual void OnTriggered(BeatEventArgs e);

    public int CompareTo(BeatTrigger other)
    {
        return Timestamp.CompareTo(other.Timestamp);
    }
}

public struct BeatHit