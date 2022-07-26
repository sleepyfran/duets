module Cli.Text.Inventory

open Entities

let itemAdded itemName =
    Styles.success $"{itemName} has been added to your inventory"

let itemRemoved itemName =
    Styles.danger $"{itemName} has been removed from your inventory"

let noItems =
    "You are not carrying anything"

let itemsCurrentlyCarrying =
    "These are the things you're currently carrying around:"

let itemRow item =
    $"""- {Styles.object item.Brand}, {Generic.itemTypeDetail item.Type}"""

let itemNotFound name =
    Styles.error $"You don't have any item called \"{name}\""

let itemNotDrinkable =
    Styles.error "You can't drink that!"

let drunkItem = Styles.success "*Gulp*"
