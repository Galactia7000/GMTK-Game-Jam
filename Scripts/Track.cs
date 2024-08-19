using Godot;
using System;
using System.Collections;

public partial class Track
{
    public Track(string song, float duration, int trackNo)
    {
        songNotes = new Queue();
        foreach(char c in song) songNotes.Enqueue(c);
        beatDuration = duration;
        timer = 0f;
        TrackNumber = trackNo;
        numberOfBeats = songNotes.Count;
    }
    Queue songNotes;
    float timer;
    public int TrackNumber;
    public int numberOfBeats;
    float beatDuration;
    public string Update(double delta)
    {
        timer += (float)delta;
        if (timer >= beatDuration)
        {
            timer = 0f;
            if(songNotes.Count > 0) return songNotes.Dequeue().ToString();
            else return "X";
        }
        return "-";
    }

}
