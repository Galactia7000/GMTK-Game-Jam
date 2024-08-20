using Godot;

public partial class NoteMovement : CharacterBody2D
{
	[Export] public float Speed = 500f;
	AudioStream sound;
	float pitchShift;
	float yPos;
	Vector2 direction = Vector2.Left;
    public override void _Ready()
    {
        yPos = Position.Y;
    }
    public override void _PhysicsProcess(double delta)
	{
		Velocity = direction * Speed * (float)delta;
		MoveAndSlide();
		Position = new Vector2(Position.X, yPos);
		GetNode<Sprite2D>("Sprite").Modulate = Color.FromHsv(Position.X / (577 + 60), 1, 0.75F);
	}
	public void SetNote(Note thisNote, float pitch)
	{
		Sprite2D sprite = GetNode<Sprite2D>("Sprite");
		sprite.Texture = thisNote.Texture;
		sprite.Position = thisNote.SpriteOffset;
		sound = thisNote.Sound;
		if(pitch > 0) pitchShift = pitch;
		else pitchShift = 0.2f;
		GetNode<CollisionShape2D>("Collider").Shape = thisNote.Collision;
	}
	public async void Play()
	{
		AudioManager.instance.Play(sound, pitchShift);
		GetNode<CpuParticles2D>("Particle").Emitting = true;
		direction = Vector2.Zero;
		GetNode<Sprite2D>("Sprite").Visible = false;
		await ToSignal(GetTree().CreateTimer(0.6), "timeout");
		QueueFree();
	}
}
