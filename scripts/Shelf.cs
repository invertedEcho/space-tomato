using System;
using Godot;

public partial class Shelf : Node2D
{
    public const String DirtPatchNormalTexturePath = "res://textures/plants/dirtpatch/dirtpatch_normal.png";
    public const String DirtPatchDryTexturePath = "res://textures/plants/dirtpatch/dirtpatch_dry.png";

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

    public Sprite2D dirtPatchSprite;
    public Timer shelfTimer;

    public override void _Ready()
    {
        base._Ready();
        area.InputPickable = true;

        Random randomInstance = new Random();
        prePlanted = randomInstance.NextDouble() >= 0.5;

        area.MouseEntered += onMouseEntered;
        area.MouseExited += onMouseExit;

        dirtPatchSprite = GetNode<Sprite2D>("graphics/dirt_sprite");
        shelfTimer = GetNode<Timer>("shelf_timer");
        shelfTimer.WaitTime = randomInstance.Next(45, 60);


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
            GD.Print("adding HandleTimeout handler because this plant has prePlanted=true | origin: _Ready()");
            shelfTimer.Timeout += HandleTimeout;
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

            dirtPatchSprite.Texture = (Texture2D)GD.Load(DirtPatchNormalTexturePath);
        }
    }

    private void HandleTimeout()
    {
        if (plantReference == null)
        {
            GD.PrintErr("handle timoeout from shelf timer was called, but no plant planted yet. ignoring!");
            return;
        }

        plantReference.HandleNextCycle();
        dirtPatchSprite.Texture = (Texture2D)GD.Load(DirtPatchDryTexturePath);
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

            if (Input.IsActionJustPressed("left_mouse_button"))
            {
                UglyGlobalState.interactionHUD.setPosition(GlobalPosition.X, GlobalPosition.Y - 64);
                UglyGlobalState.interactionHUD.Visible = true;
                UglyGlobalState.interactionHUD.clearEventData();
                UglyGlobalState.interactionHUD.optionSelected += onOptionSelected;
                if (plantReference == null)
                {
                    Texture2D tomatoTexture = (Texture2D)GD.Load("res://textures/plants/tomato/tomato_plant_with_fruits.png");
                    UglyGlobalState.interactionHUD.setTexture(tomatoTexture, 0);

                    Texture2D monsteraTexture = (Texture2D)GD.Load("res://textures/plants/monstera/monstera_plant_full.png");
                    UglyGlobalState.interactionHUD.setTexture(monsteraTexture, 1);

                    Texture2D tubaflowerTexture = (Texture2D)GD.Load("res://textures/plants/tubaflower/tubaflower_plant_full.png");
                    UglyGlobalState.interactionHUD.setTexture(tubaflowerTexture, 2);

                    Texture2D candleflowerTexture = (Texture2D)GD.Load("res://textures/plants/candleflower/candleflower_plant_full.png");
                    UglyGlobalState.interactionHUD.setTexture(candleflowerTexture, 3);
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

    // TODO: this function should be in the interactionHUD script
    // and the interactionHUD script should have a reference to the shelf
    // can we have the interactionHUD as a child of a shelf?
    // then we also dont need this in UglyGlobalState and will clean up lots of stuff
    // TODO: also needs heavy refactor
    private void onOptionSelected(int selectedOption)
    {
        UglyGlobalState.interactionHUD.Visible = false;

        UglyGlobalState.interactionHUD.optionSelected -= onOptionSelected;

        var shelfHasPlantOrCrop = plantReference != null;

        if (shelfHasPlantOrCrop)
        {
            if (selectedOption == 0)
            {
                plantReference.isWatered = true;
                dirtPatchSprite.Texture = (Texture2D)GD.Load("res://textures/plants/dirtpatch/dirtpatch_normal.png");
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
                GD.Print("destroy action selected, freeing plantreference, setting plantreference to null and removing timeouthandler from shelfTimer");
                plantReference.Free();
                plantReference = null;
                shelfTimer.Timeout -= HandleTimeout;
                UglyGlobalState.fertilizerCount += 1;
            }
        }
        else
        {
            GD.Print("Option selected on an empty shelf!");
            var plantScene = (Plant)ResourceLoader.Load<PackedScene>("res://prefabs/plant.tscn").Instantiate();
            plantReference = plantScene;

            GD.Print("adding HandleTimeout handler because plant selected on an empty shelf | origin: onOptionSelected from interactionHUD");
            shelfTimer.Timeout += HandleTimeout;
            // TODO: extract this to a function get planttype for optionOfInteractionHUD
            if (selectedOption == 0)
            {
                plantScene.plantType = PlantType.TOMATO;
            }
            else if (selectedOption == 1)
            {
                plantScene.plantType = PlantType.MONSTERA;
            }
            else if (selectedOption == 2)
            {
                plantScene.plantType = PlantType.TUBAFLOWER;
            }
            else if (selectedOption == 3)
            {
                plantScene.plantType = PlantType.CANDLE_FLOWER;
            }
            AddChild(plantScene);
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
