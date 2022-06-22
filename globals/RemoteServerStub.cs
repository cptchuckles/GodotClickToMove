using Godot;
using System;
using System.Linq;

public class RemoteServerStub : Node
{
    [Signal]
    delegate void LoginEvent(string userId, Vector2 spawnPosition);
    [Signal]
    delegate void MovementEvent(string clientId, Vector2 destination);

    public override void _Ready()
    {
        // Emit a LoginEvent for User with Id 86e7561c-d7e6-4582-becb-43a0543bac08 after 1 second
        // This gives WebsocketClient enough time to spawn in and connect to the LoginEvent signal
        // so that when it emits, something actually happens.
        // The delay is artificial.
        // I'm just pretending that the remote server responds to the client's login request, telling it
        // what its User.Id is and its spawn position -- data that would be coming from the database
        // upon a successful login attempt by a user trying to play the game.
        GetTree().CreateTimer(1).Connect("timeout", this, "emit_signal", new Godot.Collections.Array() {
                nameof(LoginEvent),
                "86e7561c-d7e6-4582-becb-43a0543bac08",
                new Vector2(360, 240)
                });
    }

    public void _OnClientMovementRequest(string clientId, Vector2 desiredDestination)
    {
        // Server contains a representation of the game world
        var character = GetTree().GetNodesInGroup("UserAvatars").Cast<Character>().First(c => c.Id == clientId);
        if (character != null)
        {
            //TODO: Do some raycast to find out if the character can move to the destination or something.

            // Relay to "all" clients that the specified user is now moving to the destination
            EmitSignal(nameof(MovementEvent), clientId, desiredDestination);
        }
    }
}
