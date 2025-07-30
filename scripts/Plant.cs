using System;
using Godot;

public enum PlantState
{
	CROP_FULL,
	CROP_DEAD,
	PLANT_FULL,
	PLANT_DRY,
	PLANT_DEAD,
}

public partial class Plant : Node2D
{
	[Export]
	public bool isWatered;
	[Export]
	public PlantState plantState;

	[Export]
	public Timer timer;

	[Export]
	public Sprite2D sprite2D;

	public override void _Ready()
	{
		timer.Timeout += HandleTimeout;
	}


	public void HandleTimeout()
	{
		var nextPlantState = GetNextPlantState();
		GD.Print("nextPlantState: " + nextPlantState);
		var spritePathForPlantState = GetSpritePathForPlantState(nextPlantState);
		sprite2D.Texture = (Texture2D)GD.Load("res://" + spritePathForPlantState);
		GD.Print("timer ended, restarting");
	}

	public String GetSpritePathForPlantState(PlantState plantState)
	{
		switch (plantState)
		{
			case PlantState.PLANT_FULL:
				return "textures/plants/tomato/tomato_plant_full.png";
			case PlantState.PLANT_DEAD:
				return "textures/plants/tomato/tomato_plant_dead.png";
			case PlantState.PLANT_DRY:
				return "textures/plants/tomato/tomato_plant_dry.png";
			case PlantState.CROP_DEAD:
				return "textures/plants/tomato/tomato_crop_dead.png";
			case PlantState.CROP_FULL:
				return "textures/plants/tomato/tomato_crop_full.png";
		}
		// this path should thereotically never be possible to reach
		GD.Print("WARN: reached impossible? case");
		return "textures/plants/tomato/tomato_plant_full.png";
	}

	public PlantState GetNextPlantState()
	{
		if (isWatered)
		{
			switch (plantState)
			{
				case PlantState.CROP_DEAD:
					return PlantState.CROP_FULL;
				case PlantState.CROP_FULL:
					return PlantState.PLANT_DRY;
				case PlantState.PLANT_DRY:
					return PlantState.PLANT_FULL;
				case PlantState.PLANT_FULL:
					return PlantState.PLANT_FULL;
				case PlantState.PLANT_DEAD:
					return PlantState.PLANT_DEAD;
			}
		}
		else
		{
			switch (plantState)
			{
				case PlantState.CROP_DEAD:
					return PlantState.CROP_DEAD;
				case PlantState.CROP_FULL:
					return PlantState.CROP_DEAD;
				case PlantState.PLANT_DEAD:
					return PlantState.PLANT_DEAD;
				case PlantState.PLANT_DRY:
					return PlantState.PLANT_DEAD;
				case PlantState.PLANT_FULL:
					return PlantState.PLANT_DRY;
			}
		}
		// this path should thereotically never be possible to reach
		GD.Print("WARN: reached impossible? case");
		return PlantState.PLANT_DRY;
	}
}
