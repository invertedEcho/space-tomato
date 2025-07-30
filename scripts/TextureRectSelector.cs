using Godot;
using System;

public partial class TextureRectSelector : TextureRect
{

    [Export]
    private TextureRect selected;
    [Export]
    private TextureRect icon;

    private bool isHovered = false;

    public double elapsedTime = 0;

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

        Modulate = new Color(1, 1, 1, Mathf.Lerp(0, 1, (float)elapsedTime * 9));

        elapsedTime += delta;
    }

    public void setIcon(Texture2D texture)
    {
        icon.Texture = texture;
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
