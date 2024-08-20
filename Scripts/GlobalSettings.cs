using Godot;
using System;

public partial class GlobalSettings : Node
{
    public static GlobalSettings instance {get; private set;}
    public override void _Ready()
    {
        instance = this;
    }

    public bool ScreenShakeEnabled { get; set; }
    public string CurrentSongName { get; set; }
}
