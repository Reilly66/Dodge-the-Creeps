using Godot;
using System;

public partial class Player : Area2D
{

	[Export]
	public int Speed {get; set; } = 400; // pixels per second for player
	public Vector2 ScreenSize;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var velocity = Vector2.Zero;

		if (Input.IsActionPressed("move_right")) velocity.X += 1;
		if (Input.IsActionPressed("move_left")) velocity.X -= 1;
		if (Input.IsActionPressed("move_up")) velocity.Y -= 1;
		if (Input.IsActionPressed("move_down")) velocity.Y += 1;

		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		if (velocity.Length()>0) {
			// if the magnitude of the velocity vector is greater than 0 a button is pressed
			velocity = velocity.Normalized() * Speed;
			// ensure that the magnitude fo the velocity vector is always equal to speed
			// thus ensuring that diagonal motion is the same speed as unidimensional motion
			animatedSprite2D.Play();
		} else {
			animatedSprite2D.Stop();
		}
		Position += velocity * (float) delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		);
	}
}
