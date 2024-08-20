using Godot;
using System;

public partial class CameraShake : Camera2D
{
    [Export] float RandomStrength;
    [Export] float ShakeFade;

    Random RNG = new Random();
    float shakeStrength;

    float trauma = 0f;
    float traumaPower = 2;

    public override void _Process(double delta)
    {
        if (shakeStrength > 0) 
        {
            shakeStrength = Mathf.Lerp(shakeStrength, 0, ShakeFade * (float)delta);
            Offset = RandomOffset();
        }
    }

    void Shake(float strength)
    {
        if (GlobalSettings.instance.ScreenShakeEnabled) shakeStrength = strength;
    }
    Vector2 RandomOffset() => new Vector2(RandomFloat(-shakeStrength, shakeStrength),RandomFloat(-shakeStrength, shakeStrength));
    float RandomFloat(float min, float max) => RNG.NextSingle() * (max - min) + min;
}
