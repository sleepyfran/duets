module Duets.Cli.Text.Items

open Duets.Entities

let itemAddedCharacterToInventory item =
    Styles.success
        $"{Generic.itemDetailedName item} has been added to your inventory"

let itemAddedToBandInventory item quantity =
    $"{quantity} {Generic.simplePluralOf (Generic.itemDetailedName item) quantity} added to your band's inventory"
    |> Styles.success

let itemRemovedFromCharacterInventory item =
    let mainProperty = item.Properties |> List.tryHead

    match mainProperty with
    | Some(Edible _) ->
        Styles.success $"You eat the {Generic.itemDetailedName item}."
    | Some(Drinkable _) ->
        Styles.success $"You take a sip of your {Generic.itemDetailedName item}."
    | _ ->
        Styles.warning $"{Generic.itemDetailedName item} has been removed from your inventory"

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

let itemNotRideable = Styles.error "Excuse me, you want to ride what?"

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
