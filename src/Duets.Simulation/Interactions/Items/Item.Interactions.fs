[<RequireQualifiedAccess>]
module Duets.Simulation.Interactions.Items

open Duets.Entities
open Duets.Simulation

/// Defines an error that happened while trying to consume an item.
type ConsumeError = ActionNotPossible

let private removeFromGameWorld state item =
    let coords = Queries.World.currentCoordinates state

    let location = Queries.Items.itemLocation state coords item

    match location with
    | ItemLocation.World -> [ ItemRemovedFromWorld(coords, item) ]
    | ItemLocation.Inventory -> [ ItemRemovedFromInventory item ]
    | ItemLocation.Nowhere ->
        [] (* This technically shouldn't happen, but let's just not remove the item. *)

let consume state (item: Item) action =
    match action with
    | ConsumableItemInteraction.Drink ->
        match item.Type with
        | Consumable(ConsumableItemType.Drink drink) ->
            Drink.drink state drink |> Ok
        | _ -> Error ActionNotPossible
    | ConsumableItemInteraction.Eat ->
        match item.Type with
        | Consumable(ConsumableItemType.Food food) -> Food.eat state food |> Ok
        | _ -> Error ActionNotPossible
    |> Result.map ((@) (removeFromGameWorld state item))

let private (|ExercisingOnGym|_|) (action, itemType) =
    match action, itemType with
    | InteractiveItemInteraction.Exercise, InteractiveItemType.GymEquipment _ ->
        Some()
    | _ -> None

let private (|PlayingDarts|_|) (action, itemType) =
    match action, itemType with
    | InteractiveItemInteraction.Play,
      InteractiveItemType.Electronics(ElectronicsItemType.Dartboard) -> Some()
    | _ -> None

let private (|PlayingPool|_|) (action, itemType) =
    match action, itemType with
    | InteractiveItemInteraction.Play,
      InteractiveItemType.Furniture(FurnitureItemType.BilliardTable) -> Some()
    | _ -> None

let private (|PlayingVideoGames|_|) (action, itemType) =
    match action, itemType with
    | InteractiveItemInteraction.Play,
      InteractiveItemType.Electronics(ElectronicsItemType.GameConsole) -> Some()
    | _ -> None

let private (|ReadingBooks|_|) (action, itemType) =
    match action, itemType with
    | InteractiveItemInteraction.Read, InteractiveItemType.Book book ->
        Some(book)
    | _ -> None

let private (|WatchingTV|_|) (action, itemType) =
    match action, itemType with
    | InteractiveItemInteraction.Watch,
      InteractiveItemType.Electronics(ElectronicsItemType.TV) -> Some()
    | _ -> None

let private nonInteractiveGameResult () =
    let characterWon = RandomGen.chance 50

    if characterWon then SimpleResult.Win else SimpleResult.Lose

let interact state (item: Item) action =
    let character = Queries.Characters.playableCharacter state

    let timeEffects =
        ItemInteraction.Interactive action
        |> Interaction.Item
        |> Queries.InteractionTime.timeRequired
        |> Time.AdvanceTime.advanceDayMoment' state

    match item.Type with
    | Interactive itemType ->
        match action, itemType with
        | ExercisingOnGym ->
            [ yield!
                  Character.Attribute.add
                      character
                      CharacterAttribute.Energy
                      Config.LifeSimulation.Energy.exerciseIncrease
              yield!
                  Character.Attribute.add
                      character
                      CharacterAttribute.Health
                      Config.LifeSimulation.Health.exerciseIncrease
              yield!
                  Skills.Improve.Common.applySkillModificationChance
                      state
                      {| Chance = 30
                         CharacterId = character.Id
                         ImprovementAmount = 1
                         Skills = [ SkillId.Fitness ] |} ]
            |> Ok
        | PlayingDarts ->
            [ nonInteractiveGameResult () |> PlayResult.Darts |> PlayResult ]
            |> Ok
        | PlayingPool ->
            [ nonInteractiveGameResult () |> PlayResult.Pool |> PlayResult ]
            |> Ok
        | PlayingVideoGames ->
            [ yield!
                  Character.Attribute.add
                      character
                      CharacterAttribute.Mood
                      Config.LifeSimulation.Mood.playingVideoGamesIncrease
              PlayResult(PlayResult.VideoGame) ]
            |> Ok
        | ReadingBooks book ->
            [ yield! Actions.Read.read item book state
              yield!
                  Character.Attribute.add
                      character
                      CharacterAttribute.Mood
                      Config.LifeSimulation.Mood.readingBookIncrease ]
            |> Ok
        | WatchingTV ->
            Character.Attribute.add
                character
                CharacterAttribute.Mood
                Config.LifeSimulation.Mood.watchingTvIncrease
            |> Ok
        | _ -> Error ActionNotPossible
    | _ -> Error ActionNotPossible
    |> Result.map (fun effects -> timeEffects @ effects)

/// Attempts to perform the given action on the item, if not possible (for example,
/// drinking food) it returns ActionNotPossible, otherwise returns the effects
/// that happened after consuming the item and  removes it from the inventory
/// or the game world in case of consumable items like food or drink.
let perform state (item: Item) action =
    match action with
    | ItemInteraction.Consumable interaction -> consume state item interaction
    | ItemInteraction.Interactive interaction -> interact state item interaction
