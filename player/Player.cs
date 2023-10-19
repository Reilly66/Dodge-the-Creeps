using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Player : Area2D
{
	// signal to detect collisions
	[Signal]
	public delegate void HitEventHandler();

	[Export]
	public int Speed {get; set; } = 400; // pixels per second for player

	public Vector2 ScreenSize;

	private static Vector2 DetectInput() {
		var direction = Vector2.Zero;
		if (Input.IsActionPressed("move_right")) direction.X += 1;
		if (Input.IsActionPressed("move_left")) direction.X -= 1;
		if (Input.IsActionPressed("move_up")) direction.Y -= 1;
		if (Input.IsActionPressed("move_down")) direction.Y += 1;
		// ensure that the magnitude of the direction vector is 1
		// thus normalising diagonal motion
		return direction.Normalized();
	}

	private void AnimatePlayer(Vector2 velocity) {
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		if (velocity.Length()>0) {
			animatedSprite2D.Play();

			if (velocity.X != 0) {
				animatedSprite2D.Animation = "walk";
				animatedSprite2D.FlipV = false;
				animatedSprite2D.FlipH = velocity.X < 0;
			} else if (velocity.Y != 0) {
				animatedSprite2D.Animation = "up";
				animatedSprite2D.FlipV = velocity.Y > 0;
			}
		} else {
			animatedSprite2D.Stop();
		}
	}

	private void MovePlayer(Vector2 velocity, float delta) {
		Position += velocity * delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		);
	}

	private void OnBodyEntered(PhysicsBody2D body)
	{
		Hide(); // Player disappears after being hit.
		EmitSignal(SignalName.Hit);
		// Must be deferred as we can't change physics properties on a physics callback.
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		Hide();
	}

	public void Start(Vector2 position) {
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var velocity = DetectInput() * Speed;
		AnimatePlayer(velocity);
		MovePlayer(velocity, (float) delta);
	}
}
