using Godot;
using System;
using System.IO;
using System.Linq;

public partial class MainMenu : CanvasLayer
{
    [Export] public HBoxContainer[] Buttons;
    [Export] public BaseButton[] Textures;
    [Export] public Label[] Labels;
    [Export] public Shader HoveringShader;
    [Export] public VBoxContainer Home;
    [Export] public VBoxContainer Settings;
    [Export] public VBoxContainer Help;
    [Export] public VBoxContainer SongSelect;
    [Export] public HBoxContainer SongDisplayTemplate;
    [Export] public Texture2D EasyTexture;
    [Export] public Texture2D MehTexture;
    [Export] public Texture2D HardTexture;
    [Export] public VBoxContainer SongList;
    string[] Songs;
    HBoxContainer[] SongDisplays;
    public override void _Ready()
    {
        // Song Loading
        string SongsFilePath = ProjectSettings.GlobalizePath("res://") + "Songs";
        Songs = Directory.GetFiles(SongsFilePath);
        for (int i = 0; i < Songs.Length; i++)
        {
            string name = Songs[i].Split('\\').Last();
            Songs[i] = name.Remove(name.Length - 4);
        } 
        SongDisplays = new HBoxContainer[Songs.Length];
        for(int i = 0; i < Songs.Length; i++)
        {
            int num = i;
            SongDisplays[num] = SongDisplayTemplate.Duplicate() as HBoxContainer;
            StreamReader reader = new StreamReader(ProjectSettings.GlobalizePath("res://") + "Songs/" + Songs[num] + ".txt");
            string difficulty = reader.ReadLine();
            int bpm = Convert.ToInt32(reader.ReadLine());
            int numberOfBeats = Convert.ToInt32(reader.ReadLine().Length);
            float songDuration = numberOfBeats * 60 / bpm;
            reader.Close();
            switch (difficulty)
            {
                case "Easy":
                    SongDisplays[num].GetNode<TextureRect>("Texture").Texture = EasyTexture;
                    SongDisplays[num].GetNode<TextureRect>("Texture").Modulate = Color.FromHtml("18c900");
                    break;
                case "Medium":
                    SongDisplays[num].GetNode<TextureRect>("Texture").Texture = MehTexture;
                    SongDisplays[num].GetNode<TextureRect>("Texture").Modulate = Color.FromHtml("ffcc00");
                    break;
                case "Hard":
                    SongDisplays[num].GetNode<TextureRect>("Texture").Texture = HardTexture;
                    SongDisplays[num].GetNode<TextureRect>("Texture").Modulate = Color.FromHtml("a80000");
                    break;
                default:
                    SongDisplays[num].GetNode<TextureRect>("Texture").Texture = EasyTexture;
                    break;
            }
            SongDisplays[num].GetNode<Label>("Name").Text = Songs[num];
            SongDisplays[num].GetNode<Label>("Duration").Text = Mathf.Floor(songDuration / 60) + "." + MathF.Floor(songDuration % 60);
            SongDisplays[num].GetNode<TextureButton>("Btn").Material = new ShaderMaterial() {Shader = HoveringShader.Duplicate() as Shader};
            SongDisplays[num].GetNode<TextureButton>("Btn").MouseEntered += () => HoveringPlay(SongDisplays[num].GetNode<TextureButton>("Btn"));
            SongDisplays[num].GetNode<TextureButton>("Btn").MouseExited += () => UnhoveringPlay(SongDisplays[num].GetNode<TextureButton>("Btn"));
            SongDisplays[num].GetNode<TextureButton>("Btn").Pressed += () => PlayClicked(Songs[num]);
            SongList.AddChild(SongDisplays[num]);
        }
        SongDisplayTemplate.QueueFree();

        // Button shader sorting
        for(int i = 0; i < Buttons.Length; i++)
        {
            Textures[i].Material = new ShaderMaterial() {Shader = HoveringShader.Duplicate() as Shader};
            Labels[i].Material = new ShaderMaterial() {Shader = HoveringShader.Duplicate() as Shader};
            if(i == 4) continue;
            int num = i;
            Textures[i].MouseEntered += () => Hovering(num);
            Textures[i].MouseExited += () => Unhovering(num);
        }
        Home.Visible = true;
        Settings.Visible = false;
        Help.Visible = false;
        SongSelect.Visible = false;
        ((ShaderMaterial)Textures[4].Material).SetShaderParameter("isActive", true);
    }

    void HoveringPlay(TextureButton button) => ((ShaderMaterial)button.Material).SetShaderParameter("isActive", true);
        
    void UnhoveringPlay(TextureButton button) => ((ShaderMaterial)button.Material).SetShaderParameter("isActive", false);

    void Hovering(int index)
    {
        ((ShaderMaterial)Textures[index].Material).SetShaderParameter("isActive", true);
        ((ShaderMaterial)Labels[index].Material).SetShaderParameter("isActive", true);
        Labels[index].Visible = true;
    }
    void Unhovering(int index)
    {
        ((ShaderMaterial)Textures[index].Material).SetShaderParameter("isActive", false);
        ((ShaderMaterial)Labels[index].Material).SetShaderParameter("isActive", false);
        Labels[index].Visible = false;
    }

    void ScreenShakeChanged(bool toggle)
    {
        GlobalSettings.instance.ScreenShakeEnabled = toggle;
        ((ShaderMaterial)Textures[4].Material).SetShaderParameter("isActive", toggle);
    }

    void PlayClicked(string songName)
    {
        GlobalSettings.instance.CurrentSongName = songName;
        GetTree().ChangeSceneToFile("res://Scenes/game_scene.tscn");
    }

    void SettingsButtonClicked()
    {
        Home.Visible = false;
        Settings.Visible = true;
    }
    void HelpButtonClicked()
    {
        Home.Visible = false;
        Help.Visible = true;
    }
    void SongButtonClicked()
    {
        Home.Visible = false;
        SongSelect.Visible = true;
    }

    void BackButtonClicked()
    {
        Help.Visible = false;
        Settings.Visible = false;
        SongSelect.Visible = false;
        Home.Visible = true;
    }
    void QuitGameClicked() => GetTree().Quit();
}
