using Godot;

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

        UglyGlobalState.soundManager.PlaySound(UglyGlobalState.soundManager.getSoundPalette("MENU"), Vector2.Zero);
    }


}
