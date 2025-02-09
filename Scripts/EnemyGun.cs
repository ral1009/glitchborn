using System;
using Godot;

public partial class EnemyGun : Node2D
{
    [Export] public PackedScene bullet_tscn;
    [Export] public float bulletSpeed = 600f;
    [Export] public float bps = 4f;
    [Export] public float bulletDamage = 30f;

    [Export] public Node2D Boss;

    private Gun gun;  // Reference to the Player's Gun

    [Export] public float health = 60f;

    private float fireRate;
    private float nextBullet;

    public override void _Ready()
    {
        fireRate = 1 / bps;
        gun = GetNode<Gun>("/root/Node2D/player/Gun");  // Reference to the player's Gun
    }

    public override void _Process(double delta)
    {
        if (nextBullet > fireRate)
        {
            RigidBody2D bullet = bullet_tscn.Instantiate<RigidBody2D>();  // Instantiating bullet
            bullet.Rotation = GlobalRotation;
            bullet.GlobalPosition = GlobalPosition;
            bullet.LinearVelocity = bullet.Transform.X * bulletSpeed;

            // Attach collision handling inside this script
            bullet.BodyEntered += (Node body) =>
            {
                if (body is topdown)  // Check for collision with the player
                {
                    GD.Print("Bullet hit the player!");
                    gun.TakeDamage(bulletDamage);  // Call TakeDamage on the player's Gun
                    bullet.QueueFree();  // Destroy the bullet
                }
            };

            GetTree().Root.AddChild(bullet);
            nextBullet = 0f;
        }
        else
        {
            nextBullet += (float)delta;
        }
    }

    public void BossTakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GD.Print("Boss Defeated!");
            Boss.QueueFree();  // Remove the boss from the scene
        }
    }
}
