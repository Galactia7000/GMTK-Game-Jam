using Godot;
using System;
using System.Collections.Generic;

public struct PitchedNote
{
    public AudioStream Stream { get; set; }
    public float Pitch { get; set; }
}

public partial class AudioManager : Node
{
    //Allows reference from everywhere in the code
    public static AudioManager instance {get; private set;}
    
    Queue<AudioStreamPlayer> availableChannels = new Queue<AudioStreamPlayer>();
    Queue<PitchedNote> audioToPlay = new Queue<PitchedNote>();
    int numberOfChannels = 12;
    public override void _Ready()
    {
        instance = this;
        for(int i = 0; i < numberOfChannels; i++)
        {
            AudioStreamPlayer player = new AudioStreamPlayer();
            AddChild(player);
            availableChannels.Enqueue(player);
            player.Finished += () => ChannelDone(player);
        }
    }

    public override void _Process(double delta)
    {
        if(audioToPlay.Count > 0 && availableChannels.Count > 0)
        {
            AudioStreamPlayer player = availableChannels.Dequeue();
            PitchedNote note = audioToPlay.Dequeue();
            player.Stream = note.Stream;
            player.PitchScale = note.Pitch;
            player.Play();
        }
    }

    public bool IsAudioPlaying() => availableChannels.Count < numberOfChannels || audioToPlay.Count > 0;

    public void Play(AudioStream sound, float pitch = 0f)
    {
        PitchedNote note = new PitchedNote(){Stream = sound, Pitch = pitch};
        audioToPlay.Enqueue(note);
    }

    private void ChannelDone(AudioStreamPlayer player)
    {
        availableChannels.Enqueue(player);
    }

    
}
