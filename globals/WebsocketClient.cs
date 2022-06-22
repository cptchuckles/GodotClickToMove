using Godot;
using System;
using System.Linq;

public class WebsocketClient : Node
{
    // This is the User.Id for whoever has logged into the game.
    private string _clientId = "86e7561c-d7e6-4582-becb-43a0543bac08";

    private RemoteServerStub _remoteServer;
    private ClickInputHandler _clickInputHandler;

    [Signal]
    delegate void MovementRequest(string userId, Vector2 desiredDestination);

    public override void _Ready()
    {
        // Pretend this is a connection to an actual remote server, listening for websocket messages
        _remoteServer = GetNode<RemoteServerStub>("/root/RemoteServerStub");
        _remoteServer.Connect("LoginEvent", this, "_OnLoginEvent");
        _remoteServer.Connect("MovementEvent", this, "_OnMovementEvent");

        // This makes the server listen to my event
        Connect(nameof(MovementRequest), _remoteServer, "_OnClientMovementRequest");
    }

    public void RequestMovement(Vector2 movementDestination)
    {
        GD.Print($"Client {_clientId} wants to move to {movementDestination}");
        EmitSignal(nameof(MovementRequest), _clientId, movementDestination);
    }

    private void _OnLoginEvent(string userId, Vector2 spawnPosition)
    {
        var character = (Character) GD.Load<PackedScene>("res://actors/Character/Character.tscn").Instance();
        character.Id = userId;
        character.Position = spawnPosition;
        GetTree().CurrentScene.AddChild(character);

        GD.Print($"User {userId} spawned in at {spawnPosition}");

        if (userId == _clientId)
        {
            var camera = (Node2D) GetTree().CurrentScene.FindNode("MainCamera");
            character.SetRemoteTransformTarget(camera);
        }
    }

    private void _OnMovementEvent(string clientId, Vector2 destination)
    {
        var character = GetTree().GetNodesInGroup("UserAvatars").Cast<Character>().First(c => c.Id == clientId);
        if (character != null)
        {
            GD.Print($"Server dictates that user {clientId} moves to {destination}");
            character.Destination = destination;
        }
    }
}
