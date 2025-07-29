using Godot;
using System;

public partial class SmoothFollow : Node2D
{

    [Export]
    private Node2D target;

    [ExportCategory("Follow Settings")]
    [Export]
    private int lerp;

    public override void _Ready()
    {
        base._Ready();

        if (target == null)
        {
            GD.PrintErr("HEY YOU FUCKER, YOU NEED TO SET SOMETHING FOR THIS TO WORK!");
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        Position = new Vector2(
            Mathf.Lerp(Position.X, target.Position.X, (float)delta * lerp),
            Mathf.Lerp(Position.Y, target.Position.Y, (float)delta * lerp)
            );
    }

}
