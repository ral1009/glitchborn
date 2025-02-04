extends CharacterBody2D

@export var speed := 200.0
@export var jump_force := -350.0
@export var gravity := 900.0

func _physics_process(delta):
	if not is_on_floor():
		velocity.y += gravity * delta

	var direction = Input.get_axis("a", "d")
	
	if direction:
		velocity.x = direction * speed
	else:
		velocity.x = move_toward(velocity.x, 0, speed * delta)

	if Input.is_action_just_pressed("w") and is_on_floor():
		velocity.y = jump_force

	move_and_slide()
