using UnityEngine; // debug only
using System;
using System.Collections.Generic;

public class Beatmap
{
    /// <summary>
    /// Instance variable contains the event triggers associated with the map.
    /// </summary>
    private List<BeatEvent> beatEvents;

    /// <summary>
    /// Instance variable contains the hits associated with the map.
    /// </summary>
    private IEnumerable<float> hits;

    /// <summary>
    /// Constructor initializes a new instance of the <see cref="Beatmap"/> class from a specified set of hits.
    /// </summary>
    /// <param name="hits">The timestamps of each hit</param>
    public Beatmap(IEnumerable<float> hits)
    {
        this.hits = hits;
        beatEvents = new List<BeatEvent>();
    }

    /// <summary>
    /// Method sets up a new set of triggers with the given offset and event handler.
    /// </summary>
    /// <param name="offset">The offset in seconds after a hit happens that this event should be raised.</param>
    /// <param name="handler">The method called when the event is raised.</param>
    public void AddOffset(float offset, EventHandler<BeatEventArgs> handler)
    {
        foreach (float hit in hits)
        {
            BeatEvent thisEvent = new BeatEvent(hit + offset);
            thisEvent.BeatEventTriggered += handler;
            beatEvents.Add(thisEvent);
        }
        beatEvents.Sort(); // TODO sorting isnt working lol
    }

    /// <summary>
    /// Method triggers all the events within a specified interval.
    /// </summary>
    /// <param name="from">The beginning time of the interval.</param>
    /// <param name="to">The end time of the interval.</param>
    public void Step(float from, float to)
    {
        int start = beatEvents.BinarySearch(new BeatEvent(from));
        int end = beatEvents.BinarySearch(new BeatEvent(to));

        if (start < 0) start = ~start;
        if (end < 0) end = ~end - 1;

        while (start > 0 && beatEvents[start - 1].Timestamp == from) start--;
        while (end < beatEvents.Count - 1 && beatEvents[end + 1].Timestamp == to) end++;

        if (end < start) return;

        foreach (BeatEvent trigger in beatEvents.GetRange(start, end - start))
        {
            trigger.Trigger(to);
        }
    }
}

/// <summary>
/// Class contains the properties necessary for handling beat events.
/// </summary>
public class BeatEventArgs : EventArgs
{
    /// <value>
    /// Property represents the number of seconds since the associated beat event was triggered.
    /// </value>
    public float SecondsElapsed { get; set; }

    /// <summary>
    /// Constructor initializes a new instance of the <see cref="BeatEventArgs"/> class with the given time passed since the event was triggered.
    /// </summary>
    /// <param name="secondsElapsed">The time in seconds since the event triggered.</param>
    public BeatEventArgs(float secondsElapsed)
    {
        SecondsElapsed = secondsElapsed;
    }
}

/// <summary>
/// Class contains properties and methods for triggering beat events
/// </summary>
public class BeatEvent : IComparable<BeatEvent>
{
    /// <summary>
    /// Instance variable represents the time at which the event will be triggered.
    /// </summary>
    public float Timestamp;

    /// <summary>
    /// Constructor initializes a new instance of the <see cref="BeatEvent"/> class with the given timestamp.
    /// </summary>
    /// <param name="timestamp">The time at which the event will be triggered.</param>
    public BeatEvent(float timestamp)
    {
        Timestamp = timestamp;
    }

    /// <summary>
    /// Method raises the <see cref="BeatEventTriggered"/> event.
    /// </summary>
    /// <param name="currentTime">The current time when the event is raised.</param>
    public void Trigger(float currentTime)
    {
        OnBeatEventTriggered(new BeatEventArgs(currentTime - Timestamp));
    }

    /// <summary>
    /// Method raises the <see cref="BeatEventTriggered"/> event.
    /// </summary>
    /// <param name="e">The <see cref="BeatEventArgs"/> to pass to this event's handler.</param>
    private void OnBeatEventTriggered(BeatEventArgs e)
    {
        BeatEventTriggered?.Invoke(this, e);
    }

    /// <summary>
    /// Event occurs when the playhead passes this trigger.
    /// </summary>
    public event EventHandler<BeatEventArgs> BeatEventTriggered;

    /// <inheritdoc cref="IComparable{T}.CompareTo(T)"/>
    public int CompareTo(BeatEvent other)
    {
        return Timestamp.CompareTo(other.Timestamp);
    }
}