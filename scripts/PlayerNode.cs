using System;
using System.Linq;
using Godot;

public partial class PlayerNode : Node2D
{

	[ExportCategory("Player Stats")]
	[Export]
	public int health = 100;

	[Export]
	public float oxygen = 100;

	[ExportCategory("References")]
	[Export]
	public HUD hud;

	[Export]
	public CanvasLayer gameOverScreen;
	[Export]
	public CanvasLayer pauseScreen;

	[Export]
	private PlayerCamera playerCamera;

	private double time;
	private double movementTime;

	private uint ticks;

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

			playerCamera.Zoom = new Vector2(0.2f, 0.2f);
		}

		if (Input.IsActionJustPressed("escape"))
		{

			if (pauseScreen.Visible)
			{
				pauseScreen.Visible = false;
				Engine.TimeScale = 1;
			}
			else
			{
				pauseScreen.Visible = true;
				Engine.TimeScale = 0;
			}

		}

		// tileCellData.Modulate = new Color(1, 1, 1, 0.3f);
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

		// oxygen -= 1;

		ticks++;

	}

	public void addHealth(int health) {
		this.health += health;
	}
	

	private void ProcessMovement()
	{
		var anyMovementChange = false;
		if (Engine.TimeScale < 0.2)
			return;

		var newPosition = Position;
		if (Input.IsActionJustPressed("move_right"))
		{
			anyMovementChange = true;
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
			anyMovementChange = true;
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
			anyMovementChange = true;
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
			anyMovementChange = true;
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
		if (anyMovementChange)
		{
			foreach (TileData tileData in UglyGlobalState.allRelevantTiles)
			{
				// GD.Print("Cleaning up previous tiledata, resetting modulate color");
				tileData.Modulate = new Color(1, 1, 1, 1);
			}
			var wallsTransparentLayer = GetNode<TileMapLayer>("/root/spaceship/environment/walls_transparent");

			var coords = wallsTransparentLayer.LocalToMap(Position);

			// GD.Print("our coords: " + coords);
			var bottomLeftSideCoordinates = wallsTransparentLayer.GetNeighborCell(coords, TileSet.CellNeighbor.BottomLeftSide);
			var bottomLeftSideTileData = wallsTransparentLayer.GetCellTileData(bottomLeftSideCoordinates);

			if (bottomLeftSideTileData != null)
			{
				UglyGlobalState.allRelevantTiles.Add(bottomLeftSideTileData);
				bottomLeftSideTileData.Modulate = new Color(1, 1, 1, 0.4f);
				var currentCoordinates = bottomLeftSideCoordinates;
				while (true)
				{
					var nextBottomLeftSideCoordinates = wallsTransparentLayer.GetNeighborCell(currentCoordinates, TileSet.CellNeighbor.TopLeftSide);
					var nextBottomLeftSideTileData = wallsTransparentLayer.GetCellTileData(nextBottomLeftSideCoordinates);
					if (nextBottomLeftSideTileData == null)
					{
						break;
					}
					currentCoordinates = nextBottomLeftSideCoordinates;
					nextBottomLeftSideTileData.Modulate = new Color(1, 1, 1, 0.4f);
				}
			}

			var bottomRightSideCoordinates = wallsTransparentLayer.GetNeighborCell(coords, TileSet.CellNeighbor.BottomRightSide);
			var bottomRightSideTileData = wallsTransparentLayer.GetCellTileData(bottomRightSideCoordinates);

			if (bottomRightSideTileData != null)
			{
				UglyGlobalState.allRelevantTiles.Add(bottomRightSideTileData);
				bottomRightSideTileData.Modulate = new Color(1, 1, 1, 0.4f);
			}
			
			// GD.Print("Found transparent wall on bottom left side coords, setting to opacity 30");
			// GD.Print("adding tiledata to all relevant tiles");

			GD.Print("\n");
		}
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

	private bool CheckIfWall(Godot.Vector2 current_vector, Godot.Vector2 target_vector)
	{
		var spaceState = GetWorld2D().DirectSpaceState;
		var query = PhysicsRayQueryParameters2D.Create(current_vector, target_vector);
		var result = spaceState.IntersectRay(query);
		return result.Count != 0;
	}
}
