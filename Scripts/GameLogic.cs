using Godot;
using System;

public partial class GameLogic : Node
{
    [Export] public Marker2D NoteSpawner;
    [Export] public PackedScene Note;
    [Export] public double SpawnTime = 2;
    private double timer = 0;
    public override void _Process(double delta)
    {
        timer += delta;
        if (timer >= SpawnTime)
        {
            Node2D thisNote = Note.Instantiate<Node2D>();
            thisNote.GlobalPosition = NoteSpawner.GlobalPosition;
            GetNode<Node2D>("Notes").AddChild(thisNote);
            timer = 0;
        }
    }
}
