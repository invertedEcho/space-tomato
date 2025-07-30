using System;
using Godot;

public partial class PlayerNode : Node2D
{

	[ExportCategory("Player Stats")]
	[Export]
	public int health = 100;

	[Export]
	public int oxygen = 100;

	[ExportCategory("References")]
	[Export]
	public HUD hud;

	[Export]
	private PlayerCamera playerCamera;

	private double time;

	public override void _Ready()
	{
		if (UglyGlobalState.player == null)
		{
			UglyGlobalState.player = this;
		}
		else
		{
			GD.PrintErr("There can only be one instance of the player");
		}
	}

	public override void _Process(double delta)
	{
		ProcessMovement();

		time += delta;

		if (time >= 1)
		{
			Tick();
			time = 0;
		}
	}

	public void takeDamage(int healthToSubstract)
	{
		health -= healthToSubstract;
		UglyGlobalState.player.playerCamera.shakeCamera(0.2);
		UglyGlobalState.player.hud.elapsedTime = 0;
	}

	private void Tick()
	{

		if (oxygen <= 0)
		{
			takeDamage(5);
		}

		GD.Print(health);

	}

	public void addHealth(int health) {
		this.health += health;
	}
	

	private void ProcessMovement()
	{
		var newPosition = Position;
		if (Input.IsActionJustPressed("move_right"))
		{
			var targetVector = newPosition + new Godot.Vector2(32, 16);
			var isTargetVectorWall = CheckIfWall(newPosition, targetVector);
			if (isTargetVectorWall)
			{
				return;
			}
			newPosition = targetVector;
		}
		else if (Input.IsActionJustPressed("move_left"))
		{
			var targetVector = newPosition + new Godot.Vector2(-32, -16);
			var isTargetVectorWall = CheckIfWall(newPosition, targetVector);
			if (isTargetVectorWall)
			{
				return;
			}
			newPosition = targetVector;
		}
		else if (Input.IsActionJustPressed("move_down"))
		{
			var targetVector = newPosition + new Godot.Vector2(-32, 16);
			var isTargetVectorWall = CheckIfWall(newPosition, targetVector);
			if (isTargetVectorWall)
			{
				return;
			}
			newPosition = targetVector;

		}
		else if (Input.IsActionJustPressed("move_up"))
		{
			var targetVector = newPosition + new Godot.Vector2(32, -16);
			var isTargetVectorWall = CheckIfWall(newPosition, targetVector);
			if (isTargetVectorWall)
			{
				return;
			}
			newPosition = targetVector;
		}

		if (newPosition != Position)
		{
			UglyGlobalState.interactionHUD.Visible = false;
		}

		Position = newPosition;
	}

	private bool CheckIfWall(Godot.Vector2 current_vector, Godot.Vector2 target_vector)
	{
		var spaceState = GetWorld2D().DirectSpaceState;
		// use global coordinates, not local to node
		var query = PhysicsRayQueryParameters2D.Create(current_vector, target_vector);
		var result = spaceState.IntersectRay(query);
		return result.Count != 0;
	}
}
