using Godot;
using System;

public partial class Main : Node2D
{

	[Export]
	public PackedScene MobScene {get; set; }
	
	private int _score;

	

	public void GameOver() {
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
		GetNode<Player>("Player").ToggleRestrict();
		GetNode<HUD>("HUD").ShowGameOver();
	}

	public void NewGame() {
		_score = 0;
		GetTree().CallGroup("mobs", Node.MethodName.QueueFree);
		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);
		GetNode<Timer>("StartTimer").Start();

		// update score display and show get ready
		var hud = GetNode<HUD>("HUD");
		hud.UpdateScore(_score);
		hud.ShowMessage("Get Ready!");
	}

	

	public void OnScoreTimerTimeout() {
		_score ++;
		GetNode<HUD>("HUD").UpdateScore(_score);
	}

	public void OnStartTimerTimeout() {
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
		GetNode<Player>("Player").ToggleRestrict();
	}

	public void OnMobTimerTimeout() {
		Mob mob = MobScene.Instantiate<Mob>();
		// spawn the mob
		AddChild(mob.SpawnMob(mob, GetNode<Player>("Player"), GetNode<PathFollow2D>("MobPath/MobSpawnLocation")));
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
