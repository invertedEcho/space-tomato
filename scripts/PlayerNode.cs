using Godot;
using System;

public partial class PlayerNode : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Console.WriteLine("test");
		var newPosition = Position;
		if (Input.IsActionJustPressed("move_right"))
		{
			newPosition = newPosition + new Vector2(32, 16);
		}
		else if (Input.IsActionJustPressed("move_left"))
		{
			newPosition = newPosition + new Vector2(-32, -16);
		}
		else if (Input.IsActionJustPressed("move_down"))
		{
			Console.WriteLine("Moving down");
			newPosition = newPosition + new Vector2(-32, 16);

		}
		else if (Input.IsActionJustPressed("move_up"))
		{
			Console.WriteLine("Moving up");
			// please keep this at -16 trust me
			newPosition = newPosition + new Vector2(32, -16);
		}
		Position = newPosition;
	}
}
