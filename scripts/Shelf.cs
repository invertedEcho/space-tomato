using System.Transactions;
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

		area.MouseEntered += onMouseEntered;
		area.MouseExited += onMouseExit;

		dirtSprite = GetNode<Sprite2D>("graphics/dirt_sprite");
		shelfTimer = GetNode<Timer>("shelf_timer");

		shelfTimer.Timeout += HandleTimeout;

		if (prePlanted)
		{
			var plantScene = (Plant)ResourceLoader.Load<PackedScene>("res://scenes/plant.tscn").Instantiate();
			plantScene.plantType = PlantType.TOMATO;
			plantScene.plantState = PlantState.PLANT_FULL;
			plantReference = plantScene;
			AddChild(plantScene);
			plantReference.plantSprite.Texture = (Texture2D)GD.Load("res://textures/plants/tomato/tomato_plant_full.png");
			isWatered = true;
			dirtSprite.Texture = (Texture2D)GD.Load("res://textures/plants/dirtpatch/dirtpatch_normal.png");
		}
		GD.Print("ready done shelf!");
	}

	private void HandleTimeout()
	{
		plantReference.plantState = plantReference.GetNextPlantState();
		var spritePathForPlantState = plantReference.GetSpritePathForPlantState(plantReference.plantState);
		plantReference.plantSprite.Texture = (Texture2D)GD.Load("res://" + spritePathForPlantState);
		isWatered = false;
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
					GD.Print("shelf was clicked, its empty!");
					Texture2D tomatoCropTexture = (Texture2D)GD.Load("res://textures/plants/tomato/tomato_plant_with_fruits.png");
					UglyGlobalState.interactionHUD.setTexture(tomatoCropTexture, 0);

					Texture2D monsteraCropTexture = (Texture2D)GD.Load("res://textures/plants/monstera/monstera_plant_full.png");
					UglyGlobalState.interactionHUD.setTexture(monsteraCropTexture, 1);

					Texture2D emptyTexture = (Texture2D)GD.Load("res://textures/dev/empty.png");
					UglyGlobalState.interactionHUD.setTexture(emptyTexture, 2);
					UglyGlobalState.interactionHUD.setTexture(emptyTexture, 3);
				}
				else
				{
					GD.Print("shelf was clicked, it has a plant or crop!");
					Texture2D waterTexture = (Texture2D)GD.Load("res://textures/dev/water.png");
					UglyGlobalState.interactionHUD.setTexture(waterTexture, 0);
					Texture2D destroyTexture = (Texture2D)GD.Load("res://textures/dev/destroy.png");
					UglyGlobalState.interactionHUD.setTexture(destroyTexture, 3);

					Texture2D emptyTexture = (Texture2D)GD.Load("res://textures/dev/empty.png");
					UglyGlobalState.interactionHUD.setTexture(emptyTexture, 1);
					UglyGlobalState.interactionHUD.setTexture(emptyTexture, 2);
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
				if (UglyGlobalState.fertilizerCount == 0)
				{
					// TODO: see string in print
					GD.Print("Do something visuallly that signals user that he doesnt have fertilizer");
				}
				else
				{
					plantReference.isFertilized = true;
					var isTomatoAndFull = plantReference.plantState == PlantState.PLANT_FULL && plantReference.plantType == PlantType.TOMATO;
					if (isTomatoAndFull)
					{
						plantReference.plantSprite.Texture = (Texture2D)GD.Load("res://textures/plants/tomato/tomato_with_fruits.png");
					}
				}
			}
			else if (selectedOption == 2)
			{
				// TODO: when eating, should also remove fruits
				UglyGlobalState.player.addHealth(10);
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
			var plantScene = (Plant)ResourceLoader.Load<PackedScene>("res://scenes/plant.tscn").Instantiate();
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
		}
	}

	private void onMouseEntered()
	{
		GD.Print("onMouseEntered!");
		isHovered = true;
	}

	private void onMouseExit()
	{
		GD.Print("onMouseExited!");
		isHovered = false;
	}

}
