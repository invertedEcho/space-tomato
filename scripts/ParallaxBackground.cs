using Godot;
using Godot.Collections;
using System;

public partial class ParallaxBackground : Node2D
{

    [Export]
    private Node2D cloudBase;
    [Export]
    private float cloudSpeed;

    [Export]
    private Node2D farStarBase;
    [Export]
    private float farStarSpeed;

    [Export]
    private Node2D middleStarBase;
    [Export]
    private float middleStarSpeed;

    [Export]
    private Node2D closeStarBase;
    [Export]
    private float closeStarSpeed;

    [Export]
    private Node2D speedStarBase;
    [Export]
    private float speedStarSpeed;

    [Export]
    private Array<Node2D> upAndDowny;

    [Export]
    private Timer planetSpawnTimer;

    [Export]
    private Array<Texture2D> planets;

    [Export]
    private Sprite2D planet;

    private double elapsedTime = 0;

    public override void _Ready()
    {
        base._Ready();

        planetSpawnTimer.Timeout += onTimeout;

        onTimeout();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);


        if (UglyGlobalState.paused)
            return;

        cloudBase.Position = cloudBase.Position - new Vector2(cloudSpeed, 0);

        if (cloudBase.Position.X < -640)
        {
            cloudBase.Position = cloudBase.Position + new Vector2(1280, 0);
        }

        farStarBase.Position = farStarBase.Position - new Vector2(farStarSpeed, 0);

        if (farStarBase.Position.X < -640)
        {
            farStarBase.Position = farStarBase.Position + new Vector2(1280, 0);
        }


        middleStarBase.Position = middleStarBase.Position - new Vector2(middleStarSpeed, 0);

        if (middleStarBase.Position.X < -640)
        {
            middleStarBase.Position = middleStarBase.Position + new Vector2(1280, 0);
        }

        closeStarBase.Position = closeStarBase.Position - new Vector2(closeStarSpeed, 0);

        if (closeStarBase.Position.X < -640)
        {
            closeStarBase.Position = closeStarBase.Position + new Vector2(1280, 0);
        }

        speedStarBase.Position = speedStarBase.Position - new Vector2(speedStarSpeed, 0);

        if (speedStarBase.Position.X < -640)
        {
            speedStarBase.Position = speedStarBase.Position + new Vector2(1280, 0);
        }

        planet.Position = planet.Position - new Vector2(farStarSpeed + 0.5f, 0);

        foreach (Node2D moving in upAndDowny)
        {

            moving.Position = moving.Position + new Vector2(0, (float)(Mathf.Sin(elapsedTime) / 5));

        }

        elapsedTime += delta;

    }

    public void onTimeout()
    {
        planet.Texture = planets.PickRandom();
        planet.Position = new Vector2(640, GD.RandRange(-150, 150));
    }

}
