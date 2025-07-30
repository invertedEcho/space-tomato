using System;
using Godot;
using Godot.Collections;

public partial class PlayerNode : Node2D
{

	[ExportGroup("Player Stats")]
	[Export]
	public int health = 100;

	[Export]
	public float oxygen = 100;

	[ExportGroup("References")]
	[Export]
	public HUD hud;

	[Export]
	public CanvasLayer gameOverScreen;
	[Export]
	public CanvasLayer pauseScreen;

	[Export]
	private PlayerCamera playerCamera;

	[Export]
	private Sprite2D playerSprite;

	private double time;
	private double movementTime;

	private uint ticks;

	[ExportGroup("Animation")]
	[Export]
	private Array<Texture2D> texturesHorizontal;
	[Export]
	private Array<Texture2D> texturesVertical;

	// ---

	private enum Direction
	{
		DIRECTION_FRONT,
		DIRECTION_BACK,
		DIRECTION_LEFT,
		DIRECTION_RIGHT
	}

	private Direction lastFacingDirection = Direction.DIRECTION_FRONT;

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

		movementTime += delta;

		if (movementTime > .2f)
		{
			ProcessMovementTick();
			movementTime = 0;
		}

		if (health <= 0)
		{
			gameOverScreen.Visible = true;
			Engine.TimeScale = 0;

			playerCamera.Zoom = new Vector2(2f, 2f);
			playerCamera._Process(0);
		}

		if (Input.IsActionJustPressed("escape"))
		{

			if (pauseScreen.Visible)
			{
				pauseScreen.Visible = false;
				UglyGlobalState.paused = false;
				Engine.TimeScale = 1;
			}
			else
			{
				pauseScreen.Visible = true;
				UglyGlobalState.paused = true;
				Engine.TimeScale = 0;
			}

		}

		RenderPlayerSprite();
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

		oxygen -= 1;

		UglyGlobalState.fertilizerCount += 5;

		ticks++;

	}

	public void addHealth(int health) {
		this.health += health;
	}
	

	private void ProcessMovement()
	{
		if (Engine.TimeScale < 0.2)
			return;

		var newPosition = Position;
		if (Input.IsActionJustPressed("move_right"))
		{
			var targetVector = newPosition + new Godot.Vector2(32, 16);
			var isTargetVectorWall = CheckIfWall(newPosition, targetVector);
			if (isTargetVectorWall)
			{
				return;
			}
			lastFacingDirection = Direction.DIRECTION_RIGHT;
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
			lastFacingDirection = Direction.DIRECTION_LEFT;
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
			lastFacingDirection = Direction.DIRECTION_BACK;
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
			lastFacingDirection = Direction.DIRECTION_FRONT;
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

		if (Engine.TimeScale < 0.2)
			return;

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

	private void RenderPlayerSprite()
	{

		int index = (int)(ticks % 4);

		switch (lastFacingDirection)
		{
			case Direction.DIRECTION_FRONT:
				{
					playerSprite.Texture = texturesVertical[index];
					playerSprite.FlipH = false;
					break;
				}
			case Direction.DIRECTION_BACK:
				{
					playerSprite.Texture = texturesHorizontal[index];
					playerSprite.FlipH = true;
					break;
				}
			case Direction.DIRECTION_LEFT:
				{
					playerSprite.Texture = texturesVertical[index];
					playerSprite.FlipH = true;
					break;
				}
			case Direction.DIRECTION_RIGHT:
				{
					playerSprite.Texture = texturesHorizontal[index];
					playerSprite.FlipH = false;
					break;
				}
		}
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
