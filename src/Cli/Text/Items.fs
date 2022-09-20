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
    $"""- {Styles.object item.Brand}, {Generic.itemNameWithDetail item}"""

let lookItem item =
    $"{Generic.indeterminateArticleFor item} {item}"

let lookItems itemDescriptions =
    $"You can see {Generic.listOf itemDescriptions id}"

let itemNotFound name =
    Styles.error $"You don't have any item called \"{name}\""

let itemNotDrinkable =
    Styles.error "You can't drink that!"

let itemNotEdible =
    Styles.error "You can't eat that!"

let itemCannotBeUsedForSleeping =
    Styles.error
        "Hmm, I'm pretty sure you can't sleep on that. Ever heard of a bed or a sofa?"

let itemCannotBePlayedWith =
    Styles.error
        "So... that's not a toy or a console, what are you trying to do?"

let itemCannotBeWatched =
    Styles.error
        "Why do you want to stare at that? It's not that interesting anyway. Try a TV"

let drunkItem = Styles.success "*Gulp*"

let ateItem = Styles.success "Hmmmm..."
