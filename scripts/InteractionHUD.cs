using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class InteractionHUD : Node2D
{

    [Export]
    private Array<TextureRectSelector> options;

    public override void _Ready()
    {
        base._Ready();

        if (UglyGlobalState.interactionHUD == null)
        {
            UglyGlobalState.interactionHUD = this;
        }
        else
        {
            GD.PrintErr("There can only be one instance of the interaction HUD");
        }
    }

    public void setTexture(Texture2D texture, int index)
    {

        options[index].setIcon(texture);

    }

    public void setPosition(float x, float y)
    {
        GlobalPosition = new Vector2(x, y);
        foreach (var option in options)
        {
            option.elapsedTime = 0;
            option.Modulate = new Color(1, 1, 1, 0);
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        for (int i = 0; i < options.Count; i++)
        {
            TextureRectSelector option = options.ElementAt(i);

            if (option.getHover() == true)
            {
                if (Input.IsActionJustPressed("interact"))
                {
                    GD.Print("Invoking optionSelected");
                    optionSelected.Invoke(i);
                }
            }
        }

    }

    public event Action<int> optionSelected;

}
