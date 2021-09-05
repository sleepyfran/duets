extends Control

# Parent that hosts the component.
export var interaction_component: NodePath


func _ready():
	var _subscription = get_node(interaction_component).connect(
		"on_interactable_object_changed", self, "_target_changed", [], CONNECT_DEFERRED
	)
	hide()


func _target_changed(interactable: Node):
	if interactable == null:
		hide()
		return

	if interactable.has_method("interaction_name"):
		$InteractLabel.text = interactable.interaction_name()

	show()
