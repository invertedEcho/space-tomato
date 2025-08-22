using Godot;
using Godot.Collections;

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

        int currentStage = Mathf.Clamp((int)Mathf.Remap(UglyGlobalState.fertilizerCount, 0, 100, 0, fillStates.Count), 0, fillStates.Count - 1);

        Texture = fillStates[currentStage];
    }

}
