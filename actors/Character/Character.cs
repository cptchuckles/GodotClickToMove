using Godot;
using System;

public class Character : KinematicBody2D
{
    public string Id = "";

    [Export]
    private float _speed = 200.0f;

    public Vector2 Destination;

    public override void _Ready()
    {
        Destination = GlobalPosition;
    }

    public override void _PhysicsProcess(float delta)
    {
        var movement = GlobalPosition.MoveToward(Destination, _speed * delta) - GlobalPosition;
        MoveAndCollide(movement);
    }

    public void SetRemoteTransformTarget(Node2D targetNode)
    {
        GetNode<RemoteTransform2D>("RemoteTransform2D").RemotePath = targetNode.GetPath();
    }
}
