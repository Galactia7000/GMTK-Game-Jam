using Godot;
using System;

public partial class StartPlatform : StaticBody2D
{
    public override void _Process(double delta)
    {
        if(Input.IsActionJustReleased("Jump")) QueueFree();
    }
}
