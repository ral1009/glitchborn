using System;
using Godot;

public partial class Gun : Node2D
{
    [Export] public PackedScene bullet_tscn;
    [Export] public float bulletSpeed = 600f;
    [Export] public float bps = 4f;
    [Export] public float bulletDamage = 5f;

    [Export] public float health = 30f;

    private float fireRate;
    private float nextBullet;

    private EnemyGun enemyGun;

    public override void _Ready()
    {
        fireRate = 1 / bps;
        enemyGun = GetNode<EnemyGun>("/root/Node2D/boss/BossGun"); // Fixed to properly reference the EnemyGun script
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionPressed("click") && nextBullet > fireRate)
        {
            RigidBody2D bullet = bullet_tscn.Instantiate<RigidBody2D>();  // Instantiating bullet
            bullet.Rotation = GlobalRotation;
            bullet.GlobalPosition = GlobalPosition;
            bullet.LinearVelocity = bullet.Transform.X * bulletSpeed;

            // Attach collision handling inside this script
            bullet.BodyEntered += (Node body) =>
            {
                if (body is Boss)  // Check for collision with BossGun (script EnemyGun)
                {
                    GD.Print("Bullet hit the Boss!");
                    enemyGun.BossTakeDamage(bulletDamage);  // Call the BossTakeDamage method from EnemyGun
                    bullet.QueueFree();  // Destroy the bullet
                }

                if (body is EnemyGun)
                {
                    GD.Print("Bullet collision");
                    bullet.QueueFree();
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

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GD.Print("You Lose");
            GetTree().Paused = true;  // Stop the game
        }
    }
}
