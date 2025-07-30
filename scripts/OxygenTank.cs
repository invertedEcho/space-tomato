using Godot;
using Godot.Collections;
using System;

public partial class OxygenTank : Sprite2D
{

    [Export]
    private Array<Texture2D> fillStates;

    public override void _Ready()
    {
        base._Ready();

        calculateState();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        calculateState();
    }

    private void calculateState()
    {
        int currentStage = (int)Mathf.Remap(UglyGlobalState.player.oxygen, 0, 100, 0, fillStates.Count);

        Texture = fillStates[currentStage];
    }

}
