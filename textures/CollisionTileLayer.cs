using Godot;
using System;

public partial class CollisionTileLayer : TileMapLayer
{

    public override void _Ready()
    {
        base._Ready();

        Modulate = new Color(1, 1, 1, 0);
    }

}
