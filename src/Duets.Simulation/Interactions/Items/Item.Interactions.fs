[<RequireQualifiedAccess>]
module Duets.Simulation.Interactions.Items

open Duets.Entities
open Duets.Simulation

/// Defines an error that happened while trying to consume an item.
type ConsumeError = ActionNotPossible

let private (|Drinking|_|) (action, item) =
    match action with
    | ItemInteraction.Drink ->
        item
        |> Item.Property.tryPick (function
            | Drinkable drink -> Some drink
            | _ -> None)
    | _ -> None

let private (|Eating|_|) (action, item) =
    match action with
    | ItemInteraction.Eat ->
        item
        |> Item.Property.tryPick (function
            | Edible food -> Some food
            | _ -> None)
    | _ -> None

let private (|ExercisingOnGym|_|) (action, item) =
    match action with
    | ItemInteraction.Exercise ->
        item
        |> Item.Property.tryPick (function
            | FitnessEquipment -> Some()
            | _ -> None)
    | _ -> None

let private (|PlayingDarts|_|) (action, item) =
    match action with
    | ItemInteraction.Play ->
        item
        |> Item.Property.tryPick (function
            | Playable(Darts) -> Some()
            | _ -> None)
    | _ -> None

let private (|PlayingBilliard|_|) (action, item) =
    match action with
    | ItemInteraction.Play ->
        item
        |> Item.Property.tryPick (function
            | Playable(Billiard) -> Some()
            | _ -> None)
    | _ -> None

let private (|PlayingVideoGames|_|) (action, item) =
    match action with
    | ItemInteraction.Play ->
        item
        |> Item.Property.tryPick (function
            | Playable(VideoGame) -> Some()
            | _ -> None)
    | _ -> None

let private (|ReadingBooks|_|) (action, item) =
    match action with
    | ItemInteraction.Read ->
        item
        |> Item.Property.tryPick (function
            | Readable(Book book) -> Some book
            | _ -> None)
    | _ -> None

let private (|WatchingTV|_|) (action, item) =
    match action with
    | ItemInteraction.Watch ->
        item
        |> Item.Property.tryPick (function
            | Watchable -> Some()
            | _ -> None)
    | _ -> None

let private nonInteractiveGameResult () =
    let characterWon = RandomGen.chance 50

    if characterWon then SimpleResult.Win else SimpleResult.Lose

/// Attempts to perform the given action on the item, if not possible (for example,
/// drinking food) it returns ActionNotPossible, otherwise returns the effects
/// that happened after consuming the item and  removes it from the inventory
/// or the game world in case of consumable items like food or drink.
let perform state (item: Item) action =
    let character = Queries.Characters.playableCharacter state

    let timeEffects =
        action
        |> Interaction.Item
        |> Queries.InteractionTime.timeRequired
        |> Time.AdvanceTime.advanceDayMoment' state

    match action, item with
    | Drinking drink -> Drink.drink state item drink |> Ok
    | Eating food -> Food.eat state item food |> Ok
    | ExercisingOnGym -> Actions.Exercise.exercise item character state |> Ok
    | PlayingDarts ->
        [ nonInteractiveGameResult () |> PlayResult.Darts |> GamePlayed ] |> Ok
    | PlayingBilliard ->
        [ nonInteractiveGameResult () |> PlayResult.Pool |> GamePlayed ] |> Ok
    | PlayingVideoGames ->
        [ yield!
              Character.Attribute.add
                  character
                  CharacterAttribute.Mood
                  Config.LifeSimulation.Mood.playingVideoGamesIncrease
          GamePlayed(PlayResult.VideoGame) ]
        |> Ok
    | ReadingBooks book ->
        [ yield BookRead(item, book)
          yield! Actions.Read.read item book state
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
    |> Result.map (fun effects -> timeEffects @ effects)
