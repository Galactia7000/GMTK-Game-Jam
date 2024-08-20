using Godot;
using System;

public partial class MainMenu : CanvasLayer
{
    [Export] public HBoxContainer[] Buttons;
    
    [Export] public TextureButton[] Textures;
    [Export] public Label[] Labels;
    [Export] public Shader HoveringShader;
    public override void _Ready()
    {
        for(int i = 0; i < Buttons.Length; i++)
        {
            int num = i;
            Buttons[i].MouseEntered += () => Hovering(num);
            Buttons[i].MouseExited += () => Unhovering(num);
            Textures[i].Material = new ShaderMaterial() {Shader = HoveringShader.Duplicate() as Shader};
            Labels[i].Material = new ShaderMaterial() {Shader = HoveringShader.Duplicate() as Shader};
        }
    }

    void Hovering(int index)
    {
        GD.Print(index);
        ((ShaderMaterial)Textures[index].Material).SetShaderParameter("isActive", true);
        ((ShaderMaterial)Labels[index].Material).SetShaderParameter("isActive", true);
    }
    void Unhovering(int index)
    {
        GD.Print(index);
        ((ShaderMaterial)Textures[index].Material).SetShaderParameter("isActive", false);
        ((ShaderMaterial)Labels[index].Material).SetShaderParameter("isActive", false);
    }
}
