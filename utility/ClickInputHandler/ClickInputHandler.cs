using Godot;
using System;

public class ClickInputHandler : Node2D
{
    private Vector2 _destination = new Vector2();

    private Sprite _clickMarker;
    private WebsocketClient _client;

    public override void _Ready()
    {
        _clickMarker = GetNode<Sprite>("ClickMarker");
        _clickMarker.SetAsToplevel(true);
        _clickMarker.Hide();

        _client = GetNode<WebsocketClient>("/root/WebsocketClient");
    }

    public override void _UnhandledInput(InputEvent inputEvent)
    {
        if (inputEvent is InputEventMouseButton click && click.Pressed)
        {
            _destination = GetGlobalMousePosition();
            _clickMarker.GlobalPosition = _destination;
            _clickMarker.Show();

            _client.RequestMovement(_destination);
        }
    }
}
