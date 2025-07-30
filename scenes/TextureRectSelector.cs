using Godot;
using System;

public partial class TextureRectSelector : TextureRect
{

    [Export]
    private TextureRect selected;

    private bool isHovered = false;

    public bool getHover()
    {
        return isHovered;
    }

    public override void _Ready()
    {
        MouseEntered += onMouseEntered;
        MouseExited += onMouseExit;
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        MouseEntered += onMouseEntered;
        MouseExited += onMouseExit;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (isHovered)
        {
            selected.Visible = true;
        }
        else
        {
            selected.Visible = false;
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
