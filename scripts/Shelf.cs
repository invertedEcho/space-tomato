using Godot;

public partial class Shelf : Node2D
{

	[ExportCategory("References")]
	[Export]
	Sprite2D selector;

	[Export]
	StaticBody2D area;

	[ExportCategory("Functionality")]
	[ExportGroup("Plant Spawning")]
	[Export]
	private PackedScene packedPlantScene;

	private Plant plantReference = null;

	[Export]
	private int distanceToNextTile = 64;

	private bool isHovered;

	public override void _Ready()
	{
		base._Ready();

		area.InputPickable = true;

		area.MouseEntered += onMouseEntered;
		area.MouseExited += onMouseExit;
	}

	public override void _ExitTree()
	{
		base._ExitTree();

		area.MouseEntered -= onMouseEntered;
		area.MouseExited -= onMouseExit;
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
				UglyGlobalState.interactionHUD.optionSelected += onOptionSelected;

				if (plantReference == null)
				{
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

		if (plantReference == null)
		{
			if (selectedOption == 0)
			{
				// tomatoCropSprite.Texture = tomatoCropTexture;

				GD.Print("empty shelf, loading plant scene into tile! type: tomato");
				var plantScene = (Plant)ResourceLoader.Load<PackedScene>("res://scenes/plant.tscn").Instantiate();
				plantScene.plantType = PlantType.TOMATO;
				plantReference = plantScene;
				AddChild(plantScene);
			}
			else if (selectedOption == 1)
			{
				GD.Print("empty shelf, loading plant scene into tile! type: tomato");
				var scene = (Plant)ResourceLoader.Load<PackedScene>("res://scenes/plant.tscn").Instantiate();
				scene.plantType = PlantType.MONSTERA;
				AddChild(scene);
			}
		}
		else
		{
			if (selectedOption == 0)
			{
				GD.Print("watering plant!");
				plantReference.isWatered = true;
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
					GD.Print("plant is now fertilized!");
					plantReference.isFertilized = true;
					var isTomatoAndFull = plantReference.plantState == PlantState.PLANT_FULL && plantReference.plantType == PlantType.TOMATO;
					if (isTomatoAndFull)
					{
						plantReference.sprite2D.Texture = (Texture2D)GD.Load("res://textures/plants/tomato/tomato_with_fruits.png");
					}
				}
			}
			else if (selectedOption == 2)
			{
				UglyGlobalState.player.addHealth(10);
				GD.Print("TODO: implement eating plant");
			}
			else if (selectedOption == 3)
			{
				plantReference.Free();
				plantReference = null;
				UglyGlobalState.fertilizerCount += 1; 
			}
			GD.Print("shelf has plant or crop");
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
