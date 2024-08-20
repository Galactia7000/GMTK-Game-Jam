using System;
using Godot;

public partial class PlayerMovement : CharacterBody2D
{
    [Signal] public delegate void PlayerJumpEventHandler(float strength);
    [Export] public AnimationTree AnimTree;
    [Export] public Sprite2D Sprite;
    [Export] public CpuParticles2D JumpParticles;

    [ExportGroup("Jumping")]
    [Export] public int JumpVelocity = 400;
    [Export] public float JumpMultiplier = 0.5f;
    [Export] public float CoyoteJumpTime = 0.25f;
    
    float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    float xPos = 75;
    float fallChange = 300;
    bool canDoubleJump = true;
    bool midJump = false;
    float coyoteTimer = 0f;
    float rgbTimer = 0f;
    AnimationNodeStateMachinePlayback animState;
    private float lightStrength = 0.2f;
    public float LightStrength { get => lightStrength; set { lightStrength = value; GetNode<PointLight2D>("Light").Energy = lightStrength; } }

    bool previousOnFloor = false;
    public override void _Ready()
    {
        animState = (AnimationNodeStateMachinePlayback)AnimTree.Get("parameters/playback");
    }
    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        if (Input.IsActionJustPressed("Jump")) 
        {
            if(IsOnFloor() || coyoteTimer < CoyoteJumpTime)
            {
                velocity.Y = -JumpVelocity;
                EmitSignal("PlayerJump", 2f);
                animState.Travel("Jump");
                JumpParticles.Emitting = true;
                coyoteTimer = CoyoteJumpTime;
                canDoubleJump = true;
                midJump = true;
            }
            else if (canDoubleJump)
            {
                velocity.Y = -JumpVelocity;
                EmitSignal("PlayerJump", 2f);
                animState.Travel("Midair Jump");
                JumpParticles.Emitting = true;
                canDoubleJump = false;
            }
        }
        if(Input.IsActionJustReleased("Jump") && velocity.Y < 0)
        {
            velocity.Y = JumpVelocity * JumpMultiplier;
        }
        if(IsOnFloor())
        {
            coyoteTimer = 0f;
            canDoubleJump = true;
            if (!previousOnFloor) 
            {
                midJump = false;
                animState.Travel("Land");
            }
        }
        else 
        {
            coyoteTimer += (float)delta;
            velocity.Y += Gravity * (float)delta;
            if (velocity.Y != 0 && !midJump) animState.Travel("Falling");
        }
        AnimTree.Set("parameters/Falling/blend_position", velocity.Y / 100f);

        previousOnFloor = IsOnFloor();
        Velocity = velocity;
        MoveAndSlide();
        Position = new Vector2(xPos, Position.Y);
        rgbTimer += (float)delta/4f;
        Modulate = Color.FromHsv(rgbTimer % 1, 1, 1);
    }
}
