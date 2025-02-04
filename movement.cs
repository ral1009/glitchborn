using Godot;

public partial class movement : CharacterBody2D
{
    [Export] private float Speed = 400.0f;
    [Export] private float Acceleration = 3000.0f;
    [Export] private float JumpForce = -500.0f;  // Start with a small jump force

    [Export] private float MaxJumpForce = -700.0f;  // Maximum jump force (negative to go upwards)
    [Export] private float Gravity = 700.0f;

    [Export] private bool Ice = false;

    private bool isJumping = false;  // Track if the player is currently jumping

    public override void _PhysicsProcess(double delta)
    {
        // Apply gravity if not on the floor
        if (!IsOnFloor())
        {
            // Continue adding gravity to the vertical velocity (Y)
            Velocity = new Vector2(Velocity.X, Velocity.Y + Gravity * (float)delta);
        }
        else
        {
            // Reset vertical velocity to 0 when on the floor (prevents bouncing or floating)
            if (Velocity.Y > 0)
            {
                Velocity = new Vector2(Velocity.X, 0);  // Stops downward motion when landing
            }
        }

        // Get horizontal movement input
        float direction = Input.GetAxis("a", "d");

        if (direction != 0)
        {
            // Apply acceleration
            Velocity = new Vector2(Mathf.MoveToward(Velocity.X, direction * Speed, Acceleration * (float)delta), Velocity.Y);
        }
        else if (Ice == false)
        {
            // Instantly stop when no keys are pressed
            Velocity = new Vector2(0, Velocity.Y);
        }
        else 
        {
            Velocity = new Vector2(Mathf.MoveToward(Velocity.X, 0, Speed * (float)delta), Velocity.Y);
        }

        // Handle jumping: only when the player is on the floor
        if (Input.IsActionPressed("w") && IsOnFloor())
        {
            // Start the jump if the player is on the floor and presses the jump key
            if (!isJumping)
            {
                isJumping = true;  // Start jumping
                JumpForce = -500.0f;  // Reset jump force to start the jump
            }

            // Gradually increase the jump force while the key is held, until the max jump force
            if (JumpForce < MaxJumpForce)
            {
                JumpForce = Mathf.MoveToward(JumpForce, MaxJumpForce, 10 * (float)delta);  // Increase jump force
            }

            // Apply the jump force (only vertical component changes)
            Velocity = new Vector2(Velocity.X, JumpForce);
        }
        else if (Input.IsActionJustReleased("w") || !IsOnFloor())
        {
            // Stop increasing jump force if the key is released or the player is not on the floor
            isJumping = false;
        }

        // Move the character
        MoveAndSlide();
    }
}
