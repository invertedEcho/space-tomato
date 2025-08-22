using Godot;

public partial class PlayerCamera : Camera2D
{
    [ExportCategory("Follow Settings")]
    [Export]
    private Node2D target;
    [Export]
    private int lerp;

    [ExportCategory("Camera Shake Settings")]
    [Export]
    private int strength;

    private double elapsedTime;
    private double endTime;

    [Export]
    public Camera2D camera;

    [Export]
    private Node2D Lighting;

    public void shakeCamera(double duration)
    {
        elapsedTime = 0;
        endTime = duration;

        UglyGlobalState.soundManager.PlaySound(UglyGlobalState.soundManager.getSoundPalette("ALERT"), Vector2.Zero);
    }

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

        if (elapsedTime < endTime)
        {
            Position = new Vector2(
                Mathf.Lerp(Position.X, target.Position.X + (strength / 2 - GD.Randf() * strength), (float)delta * lerp / 2),
                Mathf.Lerp(Position.Y, target.Position.Y + (strength / 2 - GD.Randf() * strength), (float)delta * lerp / 2)
                );
        }
        else
        {
            Position = new Vector2(
                Mathf.Lerp(Position.X, target.Position.X, (float)delta * lerp),
                Mathf.Lerp(Position.Y, target.Position.Y, (float)delta * lerp)
                );
        }

        elapsedTime += delta;

        if (Input.IsActionJustPressed("mouse_up"))
        {
            var currentZoom = camera.Zoom;
            var newZoom = currentZoom + new Vector2(0.5f, 0.5f);
            if (newZoom.X > 6 || newZoom.Y > 6)
            {
                return;
            }
            camera.Zoom = newZoom;
        }
        else if (Input.IsActionJustPressed("mouse_down"))
        {
            var currentZoom = camera.Zoom;
            var newZoom = currentZoom - new Vector2(0.5f, 0.5f);
            if (newZoom.X < 2 || newZoom.Y < 2)
            {
                return;
            }
            camera.Zoom = newZoom;
        }

    }

}
