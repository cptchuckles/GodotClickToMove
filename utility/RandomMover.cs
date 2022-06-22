using Godot;
using System;
using System.Linq;

public class RandomMover : Timer
{
    public string RemoteClientId;

    private float _range = (float) GD.RandRange(100f, 300f);
    private RemoteServerStub _remoteServer;

    public override void _Ready()
    {
        _remoteServer = GetNode<RemoteServerStub>("/root/RemoteServerStub");

        WaitTime = (float) GD.RandRange(2f, 8f);
        Connect("timeout", this, "_DoRandomMovement");
        Start();
    }

    private void _DoRandomMovement()
    {
        var character = GetTree().GetNodesInGroup("UserAvatars").Cast<Character>().First(c => c.Id == RemoteClientId);
        if (character is null)
        {
            GD.Print($"{GetPath()} has no valid Remote Client Id. Dying");
            QueueFree();
            return;
        }

        var randomPosition = character.GlobalPosition
                + new Vector2(
                    (float) GD.RandRange(-1.0, 1.0),
                    (float) GD.RandRange(-1.0, 1.0)
                ).Normalized()
                * Mathf.Sqrt(GD.Randf())
                * _range;

        _remoteServer.EmitSignal("MovementEvent", RemoteClientId, randomPosition);

        Start((float) GD.RandRange(2f, 8f));
    }
}
