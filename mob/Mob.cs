using Godot;
using System;

public partial class Mob : RigidBody2D
{
	[Export]
	public float MinMobSpeed = 200;
	[Export]
	public float MaxMobSpeed = 200;

	// Called when the node enters the scene tree for the first time.

	public Mob SpawnMob(Mob mob, Player player, PathFollow2D mobPath) {
		// create a new mob
		mob.AddToGroup("mobs", false);
		// choose a random location on the path
		// a random proportion of the total path
		var mobSpawnLocation = mobPath;
    	mobSpawnLocation.ProgressRatio = GD.Randf();

		// get position of spawn location
		Vector2 mobSpawnPosition = mobSpawnLocation.Position;
		// get the angle from the mob spawn position to the player
		float direction = player.GetDirectionToPlayer(mobSpawnPosition);
		// add some randomness to the direction
		direction += (float)GD.RandRange(-Mathf.Pi/6, Mathf.Pi/6);
		
		// set mob start position
		mob.Position = mobSpawnLocation.Position;

		// set mob velocity
		var velocity = new Vector2((float)GD.RandRange(MinMobSpeed, MaxMobSpeed), 0);
    	mob.LinearVelocity = velocity.Rotated(direction);
		return mob;
	}

	public override void _Ready()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		string[] mobTypes = animatedSprite2D.SpriteFrames.GetAnimationNames();
		animatedSprite2D.Play(mobTypes[GD.Randi() % mobTypes.Length]);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnVisibleOnScreenNotifier2DScreenExited() {
		GD.Print("Pop");
		QueueFree();
	}

}
