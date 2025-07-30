using Godot;
using System;

public partial class Shelf : Node2D
{

    [ExportCategory("References")]
    [Export]
    Sprite2D selector;

    [Export]
    StaticBody2D area;

    [Export]
    PackedScene prefabToSpawn;

    [Export]
    private int distanceToNextTile = 64;

    private bool isHovered;

    public override void _Ready()
    {
        base._Ready();

        area.InputPickable = true;

        area.MouseEntered += onMouseEntered;
        area.MouseExited += onMouseExit;
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        area.MouseEntered -= onMouseEntered;
        area.MouseExited -= onMouseExit;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (isHovered && UglyGlobalState.player.GlobalPosition.DistanceTo(GlobalPosition) < distanceToNextTile)
        {
            selector.Visible = true;
        }
        else
        {
            selector.Visible = false;
        }
    }

    private void onMouseEntered()
    {
        isHovered = true;
    }

    private void onMouseExit()
    {
        isHovered = false;
    }
    
}
