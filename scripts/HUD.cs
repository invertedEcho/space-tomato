using Godot;
using System;

public partial class HUD : Node
{

    [ExportCategory("References")]
    [Export]
    private ProgressBar healthBar;
    [Export]
    private ProgressBar oxygenBar;

    [Export]
    private Panel damageIndicator;

    private double elapsedTime = 1;

    public void setHealth(int health)
    {
        healthBar.Value = health;
    }

    public void setOxygen(int oxygen)
    {
        oxygenBar.Value = oxygen;
    }

    public void takeDamage()
    {
        elapsedTime = 0;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        damageIndicator.SelfModulate = new Color(1, 1, 1, Mathf.Lerp(1, 0, (float)elapsedTime * 2));

        elapsedTime += delta;
    }

}
