using Godot;
using System;

public partial class Main : Node2D
{

	[Export]
	public PackedScene MobScene {get; set; }
	
	private int _score;

	private static float GetDirectionToPlayer(Vector2 MobPosition, Vector2 PlayerPosition) {
		Vector2 vecToPlayer = (PlayerPosition - MobPosition).Normalized();
		if (vecToPlayer.Length() == 0) return 0;
		return Mathf.Atan2(vecToPlayer.Y, vecToPlayer.X);
	}

	public void GameOver() {
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
	}

	public void NewGame() {
		_score = 0;
		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);

		GetNode<Timer>("StartTimer").Start();
	}

	public void OnScoreTimerTimeout() {
		_score ++;
	}

	public void OnStartTimerTimeout() {
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
	}

	public void OnMobTimerTimeout() {
		// create a new mob
		Mob mob = MobScene.Instantiate<Mob>();
		
		// choose a random location on the path
		// a random proportion of the total path
		var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
    	mobSpawnLocation.ProgressRatio = GD.Randf();

		// get position of spawn location
		Vector2 mobSpawnPosition = mobSpawnLocation.Position;
		// get the angle from the mob spawn position to the player
		float direction = GetDirectionToPlayer(mobSpawnPosition, GetNode<Player>("Player").Position);
		// add some randomness to the direction
		direction += (float)GD.RandRange(-Mathf.Pi/6, Mathf.Pi/6);
		
		// set mob start position
		mob.Position = mobSpawnLocation.Position;

		// set mob velocity
		var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
    	mob.LinearVelocity = velocity.Rotated(direction);

		// spawn the mob
		AddChild(mob);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		NewGame();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
