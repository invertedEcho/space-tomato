using Godot;


public enum PlantState
{
    CROP_FULL,
    CROP_DEAD,
    PLANT_FULL,
    PLANT_DRY,
    PLANT_DEAD,
    PLANT_FRUIT
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
    public const string InitialTomatoCropTexture = "res://game/plant/textures/tomato/tomato_crop_full.png";
    public const string InitialMonsteraCropTexture = "res://game/plant/textures/monstera/monstera_crop_full.png";
    public const string InitialTubaflowerCropTexture = "res://game/plant/textures/tubaflower/tubaflower_crop_full.png";
    public const string InitialCandleflowerCropTexture = "res://game/plant/textures/candleflower/candleflower_crop_full.png";

    public const string InitialTomatoPlantTexture = "res://game/plant/textures/tomato/tomato_plant_dry.png";
    public const string InitialMonsteraPlantTexture = "res://game/plant/textures/monstera/monstera_plant_dry.png";
    public const string InitialTubaflowerPlantTexture = "res://game/plant/textures/tubaflower/tubaflower_plant_dry.png";
    public const string InitialCandleflowerPlantTexture = "res://game/plant/textures/candleflower/candleflower_plant_dry.png";

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
        if (plantType == PlantType.TOMATO)
        {
            plantSprite.Texture = (Texture2D)GD.Load(InitialTomatoCropTexture);
        }
        else if (plantType == PlantType.MONSTERA)
        {
            plantSprite.Texture = (Texture2D)GD.Load(InitialMonsteraCropTexture);
        }
        else if (plantType == PlantType.TUBAFLOWER)
        {
            plantSprite.Texture = (Texture2D)GD.Load(InitialTubaflowerCropTexture);
        }
        else if (plantType == PlantType.CANDLE_FLOWER)
        {
            plantSprite.Texture = (Texture2D)GD.Load(InitialCandleflowerCropTexture);
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        HandleOxygen(delta);
    }

    /// <summary>
    /// This method is used to handle the next cycle of this plant.
    ///
    /// It will change the plant texture to the corresponding texture depending on the next PlantState.
    ///
    /// </summary>
    public void HandleNextCycle()
    {
        plantState = GetNextPlantState();
        var spritePathForPlantState = GetSpritePathForPlantState();
        plantSprite.Texture = (Texture2D)GD.Load(spritePathForPlantState);
        isWatered = false;
    }

    private void HandleOxygen(double delta)
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

    public string GetSpritePathForPlantState()
    {
        string basePath = "res://game/plant/textures/";

        if (plantType == PlantType.TOMATO)
        {
            basePath += "tomato/";
            switch (plantState)
            {
                case PlantState.PLANT_FULL:
                    return basePath + "tomato_plant_full.png";
                case PlantState.PLANT_DEAD:
                    return basePath + "tomato_plant_dead.png";
                case PlantState.PLANT_DRY:
                    return basePath + "tomato_plant_dry.png";
                case PlantState.CROP_DEAD:
                    return basePath + "tomato_crop_dead.png";
                case PlantState.CROP_FULL:
                    return basePath + "tomato_crop_full.png";
                case PlantState.PLANT_FRUIT:
                    return basePath + "tomato_plant_with_fruits.png";
            }
        }
        else if (plantType == PlantType.MONSTERA)
        {
            basePath += "monstera/";
            switch (plantState)
            {
                // TODO: It should not be possible that a plantstate is fruit and is not a tomato.
                // but shouldnt happen right now as long as we dont set plant fruit if not tomato.
                case PlantState.PLANT_FRUIT:
                case PlantState.PLANT_FULL:
                    return basePath + "monstera_plant_full.png";
                case PlantState.PLANT_DEAD:
                    return basePath + "monstera_plant_dead.png";
                case PlantState.PLANT_DRY:
                    return basePath + "monstera_plant_dry.png";
                case PlantState.CROP_DEAD:
                    return basePath + "monstera_crop_dead.png";
                case PlantState.CROP_FULL:
                    return basePath + "monstera_crop_full.png";

            }
        }
        else if (plantType == PlantType.CANDLE_FLOWER)
        {
            basePath += "candleflower/";
            switch (plantState)
            {
                case PlantState.PLANT_FRUIT:
                case PlantState.PLANT_FULL:
                    return basePath + "candleflower_plant_full.png";
                case PlantState.PLANT_DEAD:
                    return basePath + "candleflower_plant_dead.png";
                case PlantState.PLANT_DRY:
                    return basePath + "candleflower_plant_dry.png";
                case PlantState.CROP_DEAD:
                    return basePath + "candleflower_crop_dead.png";
                case PlantState.CROP_FULL:
                    return basePath + "candleflower_crop_full.png";
            }
        }
        else if (plantType == PlantType.TUBAFLOWER)
        {
            basePath += "tubaflower/";
            switch (plantState)
            {
                case PlantState.PLANT_FRUIT:
                case PlantState.PLANT_FULL:
                    return basePath + "tubaflower_plant_full.png";
                case PlantState.PLANT_DEAD:
                    return basePath + "tubaflower_plant_dead.png";
                case PlantState.PLANT_DRY:
                    return basePath + "tubaflower_plant_dry.png";
                case PlantState.CROP_DEAD:
                    return basePath + "tubaflower_crop_dead.png";
                case PlantState.CROP_FULL:
                    return basePath + "tubaflower_crop_full.png";
            }
        }
        // this path should thereotically never be possible to reach
        GD.Print("WARN: reached impossible? case");
        return "res://game/plant/textures/tomato/tomato_plant_full.png";
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
                case PlantState.PLANT_FRUIT:
                    return PlantState.PLANT_FRUIT;
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
                case PlantState.PLANT_FRUIT:
                    return PlantState.PLANT_FULL;
            }
        }
        // TODO: find a way to do exhaustive switch
        GD.Print("WARN: reached impossible? case");
        return PlantState.PLANT_DRY;
    }
}