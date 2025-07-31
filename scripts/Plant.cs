using Godot;


public enum PlantState
{
	CROP_FULL,
	CROP_DEAD,
	PLANT_FULL,
	PLANT_DRY,
	PLANT_DEAD,
}

public enum PlantType
{
	TOMATO,
	MONSTERA,
	CANDLE_FLOWER,
	TUBAFLOWER
}

public partial class Plant : Node2D
{
	private const string DefaultTextureTomato = "res://textures/plants/tomato/tomato_crop_full.png";
	private const string DefaultTextureMonstera = "res://textures/plants/monstera/monstera_crop_full.png";
	private const string DefaultTextureTubaflower = "res://textures/plants/tubaflower/tubaflower_crop_full.png";
	private const string DefaultTextureCandleFlower = "res://textures/plants/candleflower/candleflower_crop_full.png";
	public bool isWatered = false;
	public bool isFertilized = false;

	[Export]
	public PlantState plantState;

	[Export]
	public Sprite2D plantSprite;

	[Export]
	public PlantType plantType;

	private double elapsedTime;
	private int ticks;

	public override void _Ready()
	{
		plantSprite = GetNode<Sprite2D>("plant_sprite");
		if (plantType == PlantType.TOMATO)
		{
			plantSprite.Texture = (Texture2D)GD.Load(DefaultTextureTomato);
		}
		else if (plantType == PlantType.MONSTERA)
		{
			plantSprite.Texture = (Texture2D)GD.Load(DefaultTextureMonstera);
		}
		else if (plantType == PlantType.TUBAFLOWER)
		{
			plantSprite.Texture = (Texture2D)GD.Load(DefaultTextureTubaflower);
		}
		else if (plantType == PlantType.CANDLE_FLOWER) {
			plantSprite.Texture = (Texture2D)GD.Load(DefaultTextureCandleFlower);
		}
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		handleOxygen(delta);
	}

	private void handleOxygen(double delta)
	{

		if (elapsedTime > 2 && (plantState == PlantState.PLANT_FULL || plantState == PlantState.CROP_FULL))
		{
			UglyGlobalState.player.oxygen += GetOxygenProduction();

			elapsedTime = (double)GD.Randf();
		}

		elapsedTime += delta;

	}



	private float GetOxygenProduction()
	{
		switch (plantType)
		{
			case PlantType.TOMATO:
				return 0.1f;
			case PlantType.MONSTERA:
				return 0.25f;
			case PlantType.CANDLE_FLOWER:
				return 0.1f;
			case PlantType.TUBAFLOWER:
				return 0.1f;
			default:
				return 0.2f;
		}
	}

	public string GetSpritePathForPlantState(PlantState plantState)
	{
		if (plantType == PlantType.TOMATO)
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
		}
		else if (plantType == PlantType.MONSTERA)
		{
			switch (plantState)
			{
				case PlantState.PLANT_FULL:
					return "textures/plants/monstera/monstera_plant_full.png";
				case PlantState.PLANT_DEAD:
					return "textures/plants/monstera/monstera_plant_dead.png";
				case PlantState.PLANT_DRY:
					return "textures/plants/monstera/monstera_plant_dry.png";
				case PlantState.CROP_DEAD:
					return "textures/plants/monstera/monstera_crop_dead.png";
				case PlantState.CROP_FULL:
					return "textures/plants/monstera/monstera_crop_full.png";
			}
		}
		else if (plantType == PlantType.CANDLE_FLOWER)
		{
			switch (plantState)
			{
				case PlantState.PLANT_FULL:
					return "textures/plants/candleflower/candleflower_plant_full.png";
				case PlantState.PLANT_DEAD:
					return "textures/plants/candleflower/candleflower_plant_dead.png";
				case PlantState.PLANT_DRY:
					return "textures/plants/candleflower/candleflower_plant_dry.png";
				case PlantState.CROP_DEAD:
					return "textures/plants/candleflower/candleflower_crop_dead.png";
				case PlantState.CROP_FULL:
					return "textures/plants/candleflower/candleflower_crop_full.png";
			}
		}
		else if (plantType == PlantType.TUBAFLOWER)
		{
			switch (plantState)
			{
				case PlantState.PLANT_FULL:
					return "textures/plants/tubaflower/tubaflower_plant_full.png";
				case PlantState.PLANT_DEAD:
					return "textures/plants/tubaflower/tubaflower_plant_dead.png";
				case PlantState.PLANT_DRY:
					return "textures/plants/tubaflower/tubaflower_plant_dry.png";
				case PlantState.CROP_DEAD:
					return "textures/plants/tubaflower/tubaflower_crop_dead.png";
				case PlantState.CROP_FULL:
					return "textures/plants/tubaflower/tubaflower_crop_full.png";
			}
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
