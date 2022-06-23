using Godot;
using System;
using System.Linq;

public class RemoteServerStub : Node
{
    [Signal]
    delegate void LoginEvent(string clientId, Vector2 spawnPosition);
    [Signal]
    delegate void MovementEvent(string clientId, Vector2 destination);

    public void _OnClientLoginRequest(string clientId)
    {
        // Pretend this server is doing some authentication bullshit now...

        // In practice, the spawn position would be pulled from DB, but for this example
        // just spawn in the middle of the viewport
        EmitSignal(nameof(LoginEvent), clientId, GetViewport().Size / 2f);
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
