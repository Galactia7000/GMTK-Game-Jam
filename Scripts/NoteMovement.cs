using Godot;
using System;

public partial class NoteMovement : CharacterBody2D
{
	[Export] public float Speed = 500f;
	public override void _PhysicsProcess(double delta)
	{
		Velocity = Vector2.Left * Speed * (float)delta;
		MoveAndSlide();
	}
}
