using Godot;
using System;
using System.Collections.Generic;

public partial class UglyGlobalState : Node
{

    public static PlayerNode player;
    public static InteractionHUD interactionHUD;

    public static int fertilizerCount;

    public static List<TileData> allRelevantTiles = new List<TileData>();
}
