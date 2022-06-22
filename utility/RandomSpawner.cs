using Godot;
using System;

public class RandomSpawner : Timer
{
    private RemoteServerStub _remoteServer;

    public override void _Ready()
    {
        _remoteServer = GetNode<RemoteServerStub>("/root/RemoteServerStub");
        Connect("timeout", this, "_SpawnNewRandomFucker");
    }

    private void _SpawnNewRandomFucker()
    {
        var viewport = GetViewport();
        var guid = Guid.NewGuid().ToString();

        _remoteServer.EmitSignal("LoginEvent", guid, new Vector2(
            (float) GD.RandRange(0f, viewport.Size.x),
            (float) GD.RandRange(0f, viewport.Size.y)));

        var randomMover = new RandomMover();
        randomMover.RemoteClientId = guid;
        GetTree().CurrentScene.AddChild(randomMover);

        Start((float) GD.RandRange(10f, 20f));
    }
}
