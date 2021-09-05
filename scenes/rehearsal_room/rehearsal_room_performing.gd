extends YSort


func _process(_delta):
	if Input.is_action_just_pressed("interact"):
		get_tree().change_scene("res://scenes/rehearsal_room/rehearsal_room.tscn")
