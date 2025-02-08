using Godot;
using System;

public partial class topdown : CharacterBody2D
{
    [Export]
    public int Speed { get; set; } = 200;

    [Export]
    public int DashSpeed { get; set; } = 600; // Increased speed during dash

    [Export]
    public float DashDuration { get; set; } = 0.2f; // How long the dash lasts

    [Export]
    public float DashCooldown { get; set; } = 0.5f; // Cooldown before dashing again

    private bool _isDashing = false;
    private double _dashTimer = 0;
    private double _cooldownTimer = 0;
    private Vector2 _dashDirection = Vector2.Zero; // Stores the direction of dash

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Vector2.Zero;

        if (!_isDashing) // Only allow movement if not dashing
        {
            if (Input.IsActionPressed("d")) velocity.X += 1;
            if (Input.IsActionPressed("a")) velocity.X -= 1;
            if (Input.IsActionPressed("s")) velocity.Y += 1;
            if (Input.IsActionPressed("w")) velocity.Y -= 1;

            if (velocity.Length() > 0)
                _dashDirection = velocity.Normalized(); // Store last movement direction
        }

        // Start Dash
        if (Input.IsActionJustPressed("dash") && !_isDashing && _cooldownTimer <= 0)
        {
            _isDashing = true;
            _dashTimer = DashDuration;
            _cooldownTimer = DashCooldown;
        }

        // Handle Dash Logic
        if (_isDashing)
        {
            velocity = _dashDirection * DashSpeed; // Move only in dash direction
            _dashTimer -= delta;
            if (_dashTimer <= 0)
            {
                _isDashing = false;
            }
        }
        else
        {
            velocity = velocity.Normalized() * Speed; // Normal movement
            if (_cooldownTimer > 0)
            {
                _cooldownTimer -= delta;
            }
        }

        Velocity = velocity;
        MoveAndSlide();
    }
}
