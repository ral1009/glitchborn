using Godot;

public partial class movement : CharacterBody2D
{
    [Export] private float Speed = 400.0f;
    [Export] private float Acceleration = 3000.0f;
    [Export] private float MaxJumpForce = -1000.0f;  
    [Export] private float Gravity = 700.0f;
    [Export] private bool Ice = false;

    private bool isJumping = false;  
    private bool wasJumpButtonPressed = false; // Track if the jump button was held after landing
    private float currentJumpForce = 0.0f;  

    public override void _PhysicsProcess(double delta)
    {
        // Apply gravity if not on the floor
        if (!IsOnFloor())
        {
            Velocity = new Vector2(Velocity.X, Velocity.Y + Gravity * (float)delta);
        }
        else
        {
            if (Velocity.Y > 0)
            {
                Velocity = new Vector2(Velocity.X, 0);
            }
            isJumping = false;  
        }

        // Get horizontal movement input
        float direction = Input.GetAxis("a", "d");

        if (direction != 0)
        {
            Velocity = new Vector2(Mathf.MoveToward(Velocity.X, direction * Speed, Acceleration * (float)delta), Velocity.Y);
        }
        else if (!Ice)
        {
            Velocity = new Vector2(0, Velocity.Y);
        }
        else
        {
            Velocity = new Vector2(Mathf.MoveToward(Velocity.X, 0, Speed * (float)delta), Velocity.Y);
        }

        // Check if the jump button is held
        bool isJumpButtonPressed = Input.IsActionPressed("w");

        // Prevent immediate re-jump after landing
        if (IsOnFloor() && isJumpButtonPressed)
        {
            if (!wasJumpButtonPressed)  // Only jump if the button was released before pressing again
            {
                isJumping = true;
                currentJumpForce = -10.0f;
            }
        }

        // Handle variable jump height
        if (isJumping && isJumpButtonPressed && currentJumpForce > MaxJumpForce)
        {
            currentJumpForce -= 50.0f;
            Velocity = new Vector2(Velocity.X, currentJumpForce);
        }

        if (Input.IsActionJustReleased("w") || currentJumpForce < MaxJumpForce)
        {
            isJumping = false;
        }

        wasJumpButtonPressed = isJumpButtonPressed;  // Update jump button state

        MoveAndSlide();
    }
}
