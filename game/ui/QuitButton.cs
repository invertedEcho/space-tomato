using Godot;
using System;

public partial class QuitButton : Button
{

    public override void _Ready()
    {
        base._Ready();

        ButtonDown += quit;
    }

    private void quit()
    {

		UglyGlobalState.soundManager.PlaySound(UglyGlobalState.soundManager.getSoundPalette("MENU"), Vector2.Zero);

        GetTree().Quit();


    }

}
