using Godot;
using Godot.Collections;
using System;

public partial class MusicManager : Node2D
{

    [Export]
    private Timer timer;

    [ExportGroup("Audio Sources")]
    [Export]
    private AudioStreamPlayer2D musicSource;

    [Export]
    private AudioStreamPlayer2D backgroundSource;

    [ExportGroup("Audio Files")]
    [Export]
    private Array<AudioStream> musicPieces;

    [Export]
    private Array<AudioStream> backgroundPieces;

    public override void _Ready()
    {
        base._Ready();

        timer.Timeout += onTimeoutReached;

        musicSource.Finished += onMusicFinished;
        backgroundSource.Finished += onBackgroundFinished;

        musicSource.Stream = musicPieces.PickRandom();
        musicSource.Play();
    }

    private void onTimeoutReached()
    {
        GD.Print("Timer finished, Playing a random bit of music");

        musicSource.Stream = musicPieces.PickRandom();
        musicSource.Play();
    }

    private void onMusicFinished()
    {
        GD.Print("Music Piece Finished, Starting Wait-Timer before playing next piece");

        timer.Start(GD.Randf() * 30);
    }

    private void onBackgroundFinished()
    {
        GD.Print("Background Piece finished. ANOTHER ONE!");

        //backgroundSource.Stream = backgroundPieces.PickRandom();
        //backgroundSource.Play();
    }

}
