using Godot;
using System;
using System.Numerics;

public partial class PlayerNode : Node2D
{
	public override void _Ready()
	{

	}

	public override void _Process(double delta)
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
		Position = newPosition;
	}

	public bool CheckIfWall(Godot.Vector2 current_vector, Godot.Vector2 target_vector)
	{
		var spaceState = GetWorld2D().DirectSpaceState;
		// use global coordinates, not local to node
		var query = PhysicsRayQueryParameters2D.Create(current_vector, target_vector);
		var result = spaceState.IntersectRay(query);
		return result.Count != 0;
	}
}
