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

	private double elapsedTime = 1;

	[Export]
	public TextureRect fertilizerTexture;
	[Export]
	public TextureRect waterTexture;

	public override void _Ready()
	{
	}

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

		if (Input.IsActionJustPressed("select_hotbar_1"))
		{
			GD.Print("hotbar 1 selected");
			selectedHotbarItem = 0;
			waterTexture.Texture = GD.Load<Texture2D>("textures/ui/hud/hud_selector1_selected.png");
			fertilizerTexture.Texture = GD.Load<Texture2D>("textures/ui/hud/hud_selector2.png");
		}
		else if (Input.IsActionJustPressed("select_hotbar_2"))
		{
			GD.Print("hotbar 2 selected");
			selectedHotbarItem = 1;
			fertilizerTexture.Texture = GD.Load<Texture2D>("textures/ui/hud/hud_selector2_selected.png");
			waterTexture.Texture = GD.Load<Texture2D>("textures/ui/hud/hud_selector1.png");
		}
		// else if (Input.IsActionJustPressed("select_hotbar_3"))
		// {
		// 	GD.Print("hotbar 3 selected");
		// 	selectedHotbarItem = 2;
		// 	fertilizerTexture.Texture = GD.Load<Texture2D>("textures/ui/hud/hud_selector3_selected.png");
		// }
		// else if (Input.IsActionJustPressed("select_hotbar_4"))
		// {
		// 	GD.Print("hotbar 4 selected");
		// 	selectedHotbarItem = 3;
		// 	fertilizerTexture.Texture = GD.Load<Texture2D>("textures/ui/hud/hud_selector4_selected.png");
		// }
		// else if (Input.IsActionJustPressed("select_hotbar_5"))
		// {
		// 	GD.Print("hotbar 5 selected");
		// 	selectedHotbarItem = 4;
		// 	fertilizerTexture.Texture = GD.Load<Texture2D>("textures/ui/hud/hud_selector5_selected.png");
		// }
	}

}
