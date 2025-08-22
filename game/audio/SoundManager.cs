using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class SoundManager : Node2D
{


    private Array<AudioStreamPlayer2D> audioPlayers = new Array<AudioStreamPlayer2D>();
    [Export]
    private int concurrentAudioStreams = 10;


    [ExportGroup("Sound Palette")]
    [Export]
    private Array<AudioStream> menuPalette;

    [Export]
    private Array<AudioStream> plantPalette;

    [Export]
    private Array<AudioStream> deathPalette;

    [Export]
    private Array<AudioStream> alertPalette;

    public Array<AudioStream> getSoundPalette(String palette)
    {
        switch (palette.Capitalize())
        {
            case "MENU":
                {
                    return menuPalette;
                }
            case "PLANT":
                {
                    return plantPalette;
                }
            case "DEATH":
                {
                    return deathPalette;
                }
            case "ALERT":
                {
                    return alertPalette;
                }                
        }

        return menuPalette;
    }

    public override void _Ready()
    {
        base._Ready();

        if (UglyGlobalState.soundManager == null)
        {
            UglyGlobalState.soundManager = this;
        }
        else
        {
            GD.PrintErr("There can only be one instance of the sound manager!");
            return;
        }

        for (int i = 0; i < concurrentAudioStreams; i++)
        {

            // Create seperate audio streams for playing Audio
            AudioStreamPlayer2D audioPlayer = new AudioStreamPlayer2D();
            audioPlayers.Add(audioPlayer);

            AddChild(audioPlayer, true);

        }
    }

    public void PlaySound(AudioStream sound, Vector2 position)
    {

        foreach (AudioStreamPlayer2D audioPlayer in audioPlayers)
        {

            if (audioPlayer.Playing)
            {
                continue;
                // This audio stream is busy...
                // Try the next one
            }

            GD.Print(audioPlayer.Name);

            audioPlayer.GlobalPosition = position;
            audioPlayer.Stream = sound;
            audioPlayer.Play();

            return;

        }

        GD.PrintErr("Cannot play Sound, all audiostreams are currently busy...");

    }

    public void PlaySound(Array<AudioStream> sounds, Vector2 position)
    {

        // We select a random sound and play it
        PlaySound(sounds.PickRandom(), position);

    }

}
