module Cli.Text.Items


open Entities

let itemAddedToInventory itemName =
    Styles.success $"{itemName} has been added to your inventory"

let itemRemovedFromInventory itemName =
    Styles.danger $"{itemName} has been removed from your inventory"

let noItemsInventory =
    "You are not carrying anything"

let itemsCurrentlyCarrying =
    "These are the things you're currently carrying around:"

let itemRow item =
    $"""- {Styles.object item.Brand}, {Generic.itemTypeDetail item.Type}"""

let lookItem item =
    $"{Generic.indeterminateArticleFor item} {item}"

let lookItems itemDescriptions =
    $"You can see {Generic.listOf itemDescriptions id}"

let itemNotFound name =
    Styles.error $"You don't have any item called \"{name}\""

let itemNotDrinkable =
    Styles.error "You can't drink that!"

let drunkItem = Styles.success "*Gulp*"
