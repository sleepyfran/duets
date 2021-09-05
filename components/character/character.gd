extends KinematicBody2D
class_name Character

enum Type { BASSIST, DRUMMER, GUITARIST, SINGER }

export var speed = 50
export var type = Type.GUITARIST


func _physics_process(delta):
	var velocity = Vector2()  # The player's movement vector.
	if Input.is_action_pressed("ui_right"):
		velocity.x += 1
	if Input.is_action_pressed("ui_left"):
		velocity.x -= 1
	if Input.is_action_pressed("ui_down"):
		velocity.y += 1
	if Input.is_action_pressed("ui_up"):
		velocity.y -= 1
	if velocity.length() > 0:
		velocity = velocity.normalized() * speed
		$AnimatedSprite.play()
	else:
		$AnimatedSprite.stop()
		$AnimatedSprite.frame = 0

	var _collision_info = move_and_collide(velocity * delta)

	if velocity.x != 0:
		$AnimatedSprite.animation = "walk_forward"
		$AnimatedSprite.flip_h = velocity.x < 0
	elif velocity.y != 0:
		$AnimatedSprite.animation = "walk_forward" if velocity.y > 0 else "walk_backward"
		$AnimatedSprite.flip_h = velocity.y > 0
