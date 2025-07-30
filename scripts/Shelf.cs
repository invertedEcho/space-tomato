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

            if (Input.IsActionJustPressed("interact"))
            {
                UglyGlobalState.interactionHUD.setPosition(GlobalPosition.X, GlobalPosition.Y - 64);
                UglyGlobalState.interactionHUD.Visible = true;

                UglyGlobalState.interactionHUD.optionSelected += onOptionSelected;
            }
        }
        else
        {
            selector.Visible = false;
        }


    }

    private void onOptionSelected(int option)
    {
        GD.Print("Option #" + option + " selected!");
        UglyGlobalState.interactionHUD.Visible = false;

        UglyGlobalState.interactionHUD.optionSelected -= onOptionSelected;
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
