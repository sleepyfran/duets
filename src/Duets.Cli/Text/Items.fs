module Duets.Cli.Text.Items

open Duets.Entities

let private inventoryOwner key =
    match key with
    | InventoryKey.Band -> "your band's"
    | InventoryKey.Character -> "your"

let itemAddedToInventory key itemName =

    Styles.success
        $"{itemName} has been added to {inventoryOwner key} inventory"

let itemRemovedFromInventory key itemName =
    Styles.warning
        $"{itemName} has been removed from {inventoryOwner key} inventory"

let noItemsInventory = "You are not carrying anything"

let itemsCurrentlyCarrying =
    "These are the things you're currently carrying around:"

let itemRow item =
    $"""- {Generic.itemDetailedName item}"""

let lookItem item =
    $"{Generic.indeterminateArticleFor item} {item}"

let lookItems itemDescriptions =
    $"You can see {Generic.listOf itemDescriptions id}"

let itemNotFound name =
    Styles.error $"You don't have any item called \"{name}\""

let itemNotDrinkable = Styles.error "You can't drink that!"

let itemNotEdible = Styles.error "You can't eat that!"

let itemNotReadable = Styles.error "How are you planning to read that?"

let itemCannotBeExercisedWith =
    Styles.error "You can't exercise with that! Try a treadmill"

let itemCannotBePlayedWith =
    Styles.error
        "So... that's not a toy or a console, what are you trying to do?"

let itemCannotBeWatched =
    Styles.error
        "Why do you want to stare at that? It's not that interesting anyway. Try a TV"

let drunkItem = Styles.success "*Gulp*"

let ateItem = Styles.success "Hmmmm..."

let readBook = Styles.success "You spent some time reading"

let itemAlternativeNames (item: Item) =
    let mainProperty = item.Properties |> List.head

    match mainProperty with
    | Playable(Darts) -> [ "darts" ]
    | Playable(Billiard) -> [ "pool"; "billiard" ]
    | _ -> []
