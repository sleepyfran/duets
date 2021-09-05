extends StaticBody2D


# Determines whether the given body that is currently colliding with the mic
# can interact with it.
func can_interact(body: Node):
	return body is Character and body.type == Character.Type.SINGER


# Name that will be shown when an interaction is possible.
func interaction_name():
	return "Sing"


# Starts the perform action in the current context.
func interact():
	var _result = get_tree().change_scene(
		"res://scenes/rehearsal_room/rehearsal_room_performing.tscn"
	)
