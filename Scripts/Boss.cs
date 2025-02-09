using Godot;
using System;

public partial class Boss : CharacterBody2D
{
	[Export] public Node2D player;
	[Export] public float speed = 200f;
	public override void _PhysicsProcess(double delta)
	{
		LookAt(player.Position);
		// Calculate direction to the player
        Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();

        // Set velocity towards the player
        Velocity = direction * speed;

        // Move the enemy
        MoveAndSlide();
	}
}
