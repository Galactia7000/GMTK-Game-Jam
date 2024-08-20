using Godot;
using System;

[GlobalClass]
public partial class Note : Resource
{
    [Export] public string Symbol;
    [Export] public Texture2D Texture;
    [Export] public AudioStream Sound;
    [Export] public RectangleShape2D Collision;
    [Export] public Vector2 SpriteOffset = Vector2.Zero;
}
