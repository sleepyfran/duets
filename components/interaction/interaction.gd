extends Area2D

# Signal that is emitted every time the current target changes.
signal on_interactable_object_changed(interactable)

# Parent that hosts the interaction component.
export var parent: NodePath
# Current target node that is colliding in the interact layer with the parent.
var target: Node


# In each frame attempt to interact with the current target if it's set and
# the player is pressing the interact button.
func _process(_delta):
	if target != null and Input.is_action_just_pressed("interact"):
		if target.has_method("interact"):
			target.interact()


# Method to be called when another body that is inside of the interact layer
# collides with the parent. Determines whether the colliding node can interact
# with the current one and, if so, updates the `target` to be the colliding node.
func _on_interaction_body_entered(body: Node):
	if body.has_method("can_interact") and body.can_interact(get_node(parent)):
		target = body
		emit_signal("on_interactable_object_changed", target)


# Method to be called when a body has stopped colliding with the parent. Removes
# the previous target if the previously colliding node is the same as the one
# we had saved.
func _on_interaction_body_exited(body: Node):
	if body == target:
		target = null
		emit_signal("on_interactable_object_changed", null)
