using System;
using Godot;

public partial class Shelf : Node2D
{

	[ExportCategory("References")]
	[Export]
	Sprite2D selector;

	[Export]
	StaticBody2D area;

	private Plant plantReference = null;

	[Export]
	private int distanceToNextTile = 64;

	private bool isHovered;

	[Export]
	private bool prePlanted = false;
	public bool isWatered = false;
	public Sprite2D dirtSprite;
	public Timer shelfTimer;

	public override void _Ready()
	{
		base._Ready();
		GD.Print("ready shelf!");
		area.InputPickable = true;


		Random randomBool = new Random();
		prePlanted = randomBool.NextDouble() >= 0.5;

		area.MouseEntered += onMouseEntered;
		area.MouseExited += onMouseExit;

		dirtSprite = GetNode<Sprite2D>("graphics/dirt_sprite");
		shelfTimer = GetNode<Timer>("shelf_timer");

		shelfTimer.Timeout += HandleTimeout;

		if (prePlanted)
		{
			PlantType[] plantTypes = [PlantType.CANDLE_FLOWER, PlantType.MONSTERA, PlantType.TOMATO, PlantType.TUBAFLOWER];

			var plantScene = (Plant)ResourceLoader.Load<PackedScene>("res://prefabs/plant.tscn").Instantiate();
			plantScene.plantState = PlantState.PLANT_FULL;

			Random random = new Random();
			int randomPlantTypeIndex = random.Next(0, plantTypes.Length);
			var randomPlantPick = plantTypes[randomPlantTypeIndex];
			plantScene.plantType = randomPlantPick;
			plantReference = plantScene;
			plantReference.isWatered = true;
			isWatered = true;
			AddChild(plantScene);

			switch (plantReference.plantType)
			{
				case PlantType.TOMATO:
					plantReference.plantSprite.Texture = (Texture2D)GD.Load("res://textures/plants/tomato/tomato_plant_full.png");
					break;
				case PlantType.MONSTERA:
					plantReference.plantSprite.Texture = (Texture2D)GD.Load("res://textures/plants/monstera/monstera_plant_full.png");
					break;
				case PlantType.CANDLE_FLOWER:
					plantReference.plantSprite.Texture = (Texture2D)GD.Load("res://textures/plants/candleflower/candleflower_plant_full.png");
					break;
				case PlantType.TUBAFLOWER:
					plantReference.plantSprite.Texture = (Texture2D)GD.Load("res://textures/plants/tubaflower/tubaflower_plant_full.png");
					break;
			}

			isWatered = true;
			dirtSprite.Texture = (Texture2D)GD.Load("res://textures/plants/dirtpatch/dirtpatch_normal.png");
		}
		GD.Print("ready done shelf!");
	}

	private void HandleTimeout()
	{
		if (plantReference == null)
		{
			GD.Print("handle timoeout from shelf timer was called, but no plant planted yet. ignoring!");
			return;
		}
		plantReference.plantState = plantReference.GetNextPlantState();
		var spritePathForPlantState = plantReference.GetSpritePathForPlantState(plantReference.plantState);
		plantReference.plantSprite.Texture = (Texture2D)GD.Load("res://" + spritePathForPlantState);
		isWatered = false;
		plantReference.isWatered = false;
		dirtSprite.Texture = (Texture2D)GD.Load("res://textures/plants/dirtpatch/dirtpatch_dry.png");
		GD.Print("plant timer ended, restarting. isWatered is set to false and texture was changed.");
	}

	public override void _ExitTree()
	{
		base._ExitTree();

		area.MouseEntered -= onMouseEntered;
		area.MouseExited -= onMouseExit;
		GD.Print("shelf exited tree!");
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		if (isHovered && UglyGlobalState.player.GlobalPosition.DistanceTo(GlobalPosition) < distanceToNextTile)
		{
			selector.Visible = true;

			if (Input.IsActionJustPressed("interact"))
			{
				UglyGlobalState.interactionHUD.setPosition(GlobalPosition.X, GlobalPosition.Y - 64);
				UglyGlobalState.interactionHUD.Visible = true;
				UglyGlobalState.interactionHUD.clearEventData();
				UglyGlobalState.interactionHUD.optionSelected += onOptionSelected;
				if (plantReference == null)
				{
					Texture2D tomatoCropTexture = (Texture2D)GD.Load("res://textures/plants/tomato/tomato_plant_with_fruits.png");
					UglyGlobalState.interactionHUD.setTexture(tomatoCropTexture, 0);

					Texture2D monsteraCropTexture = (Texture2D)GD.Load("res://textures/plants/monstera/monstera_plant_full.png");
					UglyGlobalState.interactionHUD.setTexture(monsteraCropTexture, 1);

					Texture2D tubaFlowerCropTexture = (Texture2D)GD.Load("res://textures/plants/tubaflower/tubaflower_plant_full.png");
					UglyGlobalState.interactionHUD.setTexture(tubaFlowerCropTexture, 2);

					Texture2D candleFlowerTexture = (Texture2D)GD.Load("res://textures/plants/candleflower/candleflower_plant_full.png");
					UglyGlobalState.interactionHUD.setTexture(candleFlowerTexture, 3);
					UglyGlobalState.soundManager.PlaySound(UglyGlobalState.soundManager.getSoundPalette("PLANT"), GlobalPosition);
				}
				else
				{
					Texture2D waterTexture = (Texture2D)GD.Load("res://textures/icons/water.png");
					UglyGlobalState.interactionHUD.setTexture(waterTexture, 0);

					if (UglyGlobalState.fertilizerCount == 0)
					{
						Texture2D emptyTexture = (Texture2D)GD.Load("res://textures/dev/empty.png");
						UglyGlobalState.interactionHUD.setTexture(emptyTexture, 1);
					}
					else
					{
						Texture2D fertilizeTexture = (Texture2D)GD.Load("res://textures/icons/fertilize.png");
						UglyGlobalState.interactionHUD.setTexture(fertilizeTexture, 1);
					}

					if (plantReference.plantState == PlantState.PLANT_FRUIT && plantReference.plantType == PlantType.TOMATO)
					{
						Texture2D eatTexture = (Texture2D)GD.Load("res://textures/icons/eat.png");
						UglyGlobalState.interactionHUD.setTexture(eatTexture, 2);
					}
					else
					{
						Texture2D emptyTexture = (Texture2D)GD.Load("res://textures/dev/empty.png");
						UglyGlobalState.interactionHUD.setTexture(emptyTexture, 2);
					}


					Texture2D destroyTexture = (Texture2D)GD.Load("res://textures/icons/destroy.png");
					UglyGlobalState.interactionHUD.setTexture(destroyTexture, 3);
				}
			}
		}
		else
		{
			selector.Visible = false;
		}


	}

	private void onOptionSelected(int selectedOption)
	{
		UglyGlobalState.interactionHUD.Visible = false;

		UglyGlobalState.interactionHUD.optionSelected -= onOptionSelected;

		var shelfHasPlantOrCrop = plantReference != null;

		if (shelfHasPlantOrCrop)
		{
			GD.Print("Option selected, shelf its coming from has plant or crop.");
			if (selectedOption == 0)
			{
				plantReference.isWatered = true;
				dirtSprite.Texture = (Texture2D)GD.Load("res://textures/plants/dirtpatch/dirtpatch_normal.png");
			}
			else if (selectedOption == 1)
			{
				if (UglyGlobalState.fertilizerCount != 0)
				{
					plantReference.isFertilized = true;
					var isTomatoAndFull = plantReference.plantState == PlantState.PLANT_FULL && plantReference.plantType == PlantType.TOMATO;
					if (isTomatoAndFull)
					{
						// TODO: actually keep tomato with fruits if once there and kept watered
						plantReference.plantSprite.Texture = (Texture2D)GD.Load("res://textures/plants/tomato/tomato_plant_with_fruits.png");
						UglyGlobalState.fertilizerCount -= 1;
						plantReference.plantState = PlantState.PLANT_FRUIT;
					}
				}
			}
			else if (selectedOption == 2)
			{
				var isTomatoAndHasFruits = plantReference.plantType == PlantType.TOMATO && plantReference.plantState == PlantState.PLANT_FRUIT;
				if (plantReference.plantType == PlantType.TOMATO)
				{

					UglyGlobalState.player.addHealth(10);
					plantReference.plantSprite.Texture = (Texture2D)GD.Load("res://textures/plants/tomato/tomato_plant_full.png");
					plantReference.plantState = PlantState.PLANT_FULL;
				}
			}
			else if (selectedOption == 3)
			{
				GD.Print("shelf has plant, destroyed action, freeing plantreference and set to null");
				plantReference.Free();
				plantReference = null;
				UglyGlobalState.fertilizerCount += 1;
			}
		}
		else
		{
			GD.Print("Option selected on an empty shelf!");
			var plantScene = (Plant)ResourceLoader.Load<PackedScene>("res://prefabs/plant.tscn").Instantiate();
			GD.Print("plantScene: " + plantScene);
			if (selectedOption == 0)
			{
				plantScene.plantType = PlantType.TOMATO;
				plantReference = plantScene;
				AddChild(plantScene);
			}
			else if (selectedOption == 1)
			{
				plantScene.plantType = PlantType.MONSTERA;
				plantReference = plantScene;
				AddChild(plantScene);
			}
			else if (selectedOption == 2)
			{
				plantScene.plantType = PlantType.TUBAFLOWER;
				plantReference = plantScene;
				AddChild(plantScene);
			}
			else if (selectedOption == 3)
			{
				plantScene.plantType = PlantType.CANDLE_FLOWER;
				plantReference = plantScene;
				AddChild(plantScene);
			}
		}
	}

	private void onMouseEntered()
	{
		isHovered = true;
	}

	private void onMouseExit()
	{
		isHovered = false;
	}

}
