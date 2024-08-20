using Godot;
using System;
using Godot.Collections;
using System.IO;

public partial class GameLogic : Node
{
    [Signal] public delegate void NoteEndEventHandler(float strength);

    [Export] public Node2D NoteSpawner;
    [Export] public PackedScene NoteScene;
    [Export] public double SpawnTime = 2;
    [Export] public Note[] NoteList;
    [Export] ShaderMaterial bgShader;
    [Export] PlayerMovement player;
    Track[] noteTracks;
    float shaderTimer = 0f;
    bool songDone = false;
    float songDuration;
    public override void _Ready()
    {
        // File Loading
        StreamReader reader = new StreamReader(ProjectSettings.GlobalizePath("res://") + "Songs/" + GlobalSettings.instance.CurrentSongName + ".txt");
        reader.ReadLine();
        int bpm = Convert.ToInt32(reader.ReadLine());
        // Track creation
        noteTracks = new Track[9];
        for(int i = 0; i < noteTracks.Length; i++)
        {
            noteTracks[i] = new Track(reader.ReadLine(), 60 / bpm, i);
        }
        songDuration = noteTracks[0].numberOfBeats * 60 / bpm;
        bgShader.SetShaderParameter("songDuration", songDuration);
        reader.Close();
    }
    public override void _Process(double delta)
    {
        shaderTimer += (float)delta;
        bgShader.SetShaderParameter("elapsedTime", shaderTimer);
        player.LightStrength = 1 + shaderTimer / songDuration;

        if(songDone)
        {
            if(GetNode("Notes").GetChildCount() == 0 && !AudioManager.instance.IsAudioPlaying()) GetTree().CallDeferred("change_scene_to_file", "res://Scenes/main_menu.tscn");
            return;
        }

        for (int i = 0; i < noteTracks.Length; i++)
        {
            char thisChar = noteTracks[i].Update(delta)[0];
            if(thisChar == '-') continue;
            if(thisChar == 'X')
            {
                songDone = true;
                break;
            }
            Note thisNote = null;
            foreach (Note note in NoteList) if(note.Symbol[0] == thisChar) thisNote = note;
            if(thisNote is null) return;
            NoteMovement thisNoteNode = NoteScene.Instantiate<NoteMovement>();
            thisNoteNode.SetNote(thisNote, i / 4f);
            thisNoteNode.GlobalPosition = NoteSpawner.GetChild<Marker2D>(noteTracks[i].TrackNumber).GlobalPosition;
            GetNode("Notes").AddChild(thisNoteNode);
        }
    }

    public void NoteHitsEnd(NoteMovement body)
    {
        body.Play();
        EmitSignal("NoteEnd", 4f);
    }
    public void PlayerDies(Node2D body)
    {
        GetTree().CallDeferred("change_scene_to_file", "res://Scenes/main_menu.tscn");
    }
}
