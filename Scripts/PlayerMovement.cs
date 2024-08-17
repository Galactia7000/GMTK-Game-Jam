using System;
using Godot;

public partial class PlayerMovement : CharacterBody2D
{
    [ExportGroup("Jumping")]
    [Export] public int JumpVelocity = 400;
    [Export] public float JumpMultiplier = 1;
    [Export] public float MaxJumpTime = 1;
    [Export] public float FallMultiplier = 1;
    float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    float xPos = 100;
    double jumpTimer = 0;
    bool jumping = false;

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        if (Input.IsActionJustPressed("Jump") && IsOnFloor()) 
        {
            jumping = true;
            velocity.Y = -JumpVelocity;
            jumpTimer = 0;
        }
        if (velocity.Y < 0 && jumping)
        {
            jumpTimer += delta;
            if(jumpTimer > MaxJumpTime) jumping = false;
            velocity.Y += Gravity * JumpMultiplier * (float)delta;

        }
        if(Input.IsActionJustReleased("Jump"))
        {
            jumping = false;
        }
        if (!IsOnFloor() && velocity.Y > 0) 
        {
            velocity.Y += Gravity * FallMultiplier * (float)delta;
        }
        else velocity.Y += Gravity * (float)delta;
        Velocity = velocity;
        MoveAndSlide();
        Position = new Vector2(xPos, Position.Y);

    }
}
