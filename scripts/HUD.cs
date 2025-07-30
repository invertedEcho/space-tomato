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

	[Export]
	public int selectedHotbarItem;

	public double elapsedTime = 1;

	[Export]
	public TextureRect fertilizerTexture;
	[Export]
	public TextureRect waterTexture;


	public override void _Process(double delta)
	{
		base._Process(delta);

		damageIndicator.SelfModulate = new Color(1, 1, 1, Mathf.Lerp(1, 0, (float)elapsedTime * 2));

		elapsedTime += delta;
		healthBar.Value = UglyGlobalState.player.health;
		oxygenBar.Value = UglyGlobalState.player.oxygen;
	}

}
