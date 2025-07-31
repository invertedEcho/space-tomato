using Godot;
using System;

public partial class HUD : CanvasLayer
{
	[ExportCategory("References")]
	[Export]
	private ProgressBar healthBar;

	[Export]
	private TextureRect damageIndicator;

	[Export]
	public int selectedHotbarItem;

	public double elapsedTime = 1;

	[Export]
	public TextureRect fertilizerTexture;
	[Export]
	public TextureRect waterTexture;


	public override void _Ready()
	{
		base._Ready();

		this.Visible = true;
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		if (elapsedTime == 0)
		{
			UglyGlobalState.soundManager.PlaySound(UglyGlobalState.soundManager.getSoundPalette("ALERT"), Vector2.Zero);
		}

		damageIndicator.SelfModulate = new Color(1, 1, 1, Mathf.Lerp(1, 0, (float)elapsedTime * 2));

		elapsedTime += delta;
		healthBar.Value = UglyGlobalState.player.health;
	}

}
