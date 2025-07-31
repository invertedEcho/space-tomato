using Godot;
using System;

public partial class ResumeButton : Button
{

    [Export]
    CanvasLayer root;

    public override void _Ready()
    {
        base._Ready();

        ButtonDown += resume;
    }

    private void resume()
    {
        Engine.TimeScale = 1;
        root.Visible = false;
        UglyGlobalState.paused = false;
    }


}
