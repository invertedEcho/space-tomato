using Godot;

public partial class SwitchSceneButton : Button
{

    [Export]
    private Resource scene;

    public override void _Ready()
    {
        base._Ready();

        ButtonDown += switchScene;
    }

    private void switchScene()
    {
        GetTree().ChangeSceneToFile(scene.ResourcePath);
    }

}
