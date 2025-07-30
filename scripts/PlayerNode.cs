using Godot;
using System;
using System.Numerics;

public partial class PlayerNode : Node2D
{

	[ExportCategory("Player Stats")]
	[Export]
	private int health = 100;

	[Export]
	private int oxygen = 100;

	[ExportCategory("References")]
	[Export]
	private HUD hud;

	[Export]
	private PlayerCamera playerCamera;

	private double time;
	private double movementTime;

	private uint ticks;

	public override void _Ready()
	{
		setHealth(health);
		setOxygen(oxygen);

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

		movementTime += delta;

		if (movementTime > .2f)
		{
			ProcessMovementTick();
			movementTime = 0;
		}

	}

	private void Tick()
	{

		if (oxygen <= 0)
		{
			setHealth(health - 5);
		}

		setOxygen(oxygen - 1);

		GD.Print(health);

		ticks++;

	}

	public void setHealth(int health)
	{
		hud.setHealth(health);

		if (health < this.health)
		{
			playerCamera.shakeCamera(0.2);
			hud.takeDamage();
		}

		this.health = health;
	}

	public void setOxygen(int oxygen)
	{
		hud.setOxygen(oxygen);
		this.oxygen = oxygen;
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
			movementTime = 0;
		}

		Position = newPosition;
	}

		private void ProcessMovementTick()
	{
		var newPosition = Position;
		if (Input.IsActionPressed("move_right"))
		{
			var targetVector = newPosition + new Godot.Vector2(32, 16);
			var isTargetVectorWall = CheckIfWall(newPosition, targetVector);
			if (isTargetVectorWall)
			{
				return;
			}
			newPosition = targetVector;
		}
		else if (Input.IsActionPressed("move_left"))
		{
			var targetVector = newPosition + new Godot.Vector2(-32, -16);
			var isTargetVectorWall = CheckIfWall(newPosition, targetVector);
			if (isTargetVectorWall)
			{
				return;
			}
			newPosition = targetVector;
		}
		else if (Input.IsActionPressed("move_down"))
		{
			var targetVector = newPosition + new Godot.Vector2(-32, 16);
			var isTargetVectorWall = CheckIfWall(newPosition, targetVector);
			if (isTargetVectorWall)
			{
				return;
			}
			newPosition = targetVector;

		}
		else if (Input.IsActionPressed("move_up"))
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
