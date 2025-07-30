using Godot;
using Godot.Collections;
using System;

public partial class FertiliserTank : Sprite2D
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

        if (UglyGlobalState.fertilizerCount > 100)
        {
            UglyGlobalState.fertilizerCount = 100;
        }

        int currentStage = (int)Mathf.Remap(UglyGlobalState.fertilizerCount, 0, 100, 0, fillStates.Count);

        Texture = fillStates[currentStage];
    }

}
