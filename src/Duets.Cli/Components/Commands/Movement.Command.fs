namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Cli.Text.World
open Duets.Common
open Duets.Entities
open Duets.Simulation.Navigation

[<RequireQualifiedAccess>]
module MovementCommand =
    /// Creates a set of commands with the available direction as the name which,
    /// when executed, moves the player towards that direction.
    let create direction roomId =
        let commandName =
            match direction with
            | North -> "n"
            | NorthEast -> "ne"
            | East -> "e"
            | SouthEast -> "se"
            | South -> "s"
            | SouthWest -> "sw"
            | West -> "w"
            | NorthWest -> "nw"

        { Name = commandName
          Description =
            $"Allows you to move to the {World.directionName direction}"
          Handler =
            fun _ ->
                $"You make your way to the {World.directionName direction}..."
                |> showMessage

                wait 1000<millisecond>

                let result = State.get () |> Navigation.enter roomId

                match result with
                | Ok effect -> Effect.apply effect
                | Error RoomEntranceError.CannotEnterStageOutsideConcert ->
                    $"""Initially the people in the bar were looking weirdly at you thinking what were you doing in there. Then the {Styles.person "bouncer"} came and kicked you out warning you {Styles.danger
                                                                                                                                                                                                       "not to get in the stage again if you're not part of the band playing"}"""
                    |> showMessage
                | Error RoomEntranceError.CannotEnterBackstageOutsideConcert ->
                    $"""You tried to sneak into the {Styles.place "backstage"}, but the bouncers catch you as soon as you enter and kicked you out warning you {Styles.danger "not to enter in there if you're not part of the band playing"}"""
                    |> showMessage
                | Error RoomEntranceError.CannotEnterHotelRoomWithoutBooking ->
                    Styles.error
                        "You cannot enter the hotel room without booking it first"
                    |> showMessage

                    Styles.information
                        "Try to use your phone to book it or head to the lobby to pay for the room"
                    |> showMessage
                | Error(RoomEntranceError.CannotEnterWithoutRequiredItems items) ->
                    let requiredItems =
                        items
                        |> List.map (fun item ->
                            let itemName = Generic.itemName item
                            $"{Generic.indeterminateArticleFor itemName} {itemName}")

                    Styles.error
                        $"You don't have the necessary items to enter. You need {Generic.listOf requiredItems id}"
                    |> showMessage

                Scene.WorldAfterMovement }

[<RequireQualifiedAccess>]
module GoOutCommand =
    /// Creates a command that allows the player to go out of a place towards
    /// the street that the place connects to.
    let create streetId =
        { Name = "go out"
          Description = "Allows you to go out to the street"
          Handler =
            fun _ ->
                "You open the door to outside..." |> showMessage

                wait 1000<millisecond>

                State.get () |> Navigation.exitTo streetId |> Effect.apply

                Scene.WorldAfterMovement }

[<RequireQualifiedAccess>]
module GoToCommand =
    /// Creates a command that allows the player to go to a different street
    /// that is connecting to the current one.
    let create (connectingStreets: Street list) =
        { Name = "go to"
          Description =
            $"""Allows you to go to a connecting street. Use as {Styles.information "go to {street name}"}"""
          Handler =
            fun args ->
                let input = args |> String.concat " "

                let matchingStreet =
                    connectingStreets
                    |> List.tryFind (fun street ->
                        String.diacriticInsensitiveContains street.Name input)

                match matchingStreet with
                | Some street ->
                    $"You make your way to {street.Name}..." |> showMessage

                    wait 1000<millisecond>

                    State.get ()
                    |> Navigation.moveTo street.Id
                    |> Result.iter Effect.apply

                    Scene.WorldAfterMovement
                | None ->
                    $"""There are no streets named "{input}" here! Use {Styles.information "look"} to see the available streets"""
                    |> Styles.error
                    |> showMessage

                    Scene.World }

[<RequireQualifiedAccess>]
module EnterCommand =
    /// Creates a command that allows the player to go enter a place.
    let create (place: Place) =
        { Name = "enter"
          Description =
            $"""Allows you to enter inside a place. Use as {Styles.information "enter {place name}"}"""
          Handler =
            fun args ->
                let input = args |> String.concat " "

                let nameMatches =
                    String.diacriticInsensitiveContains place.Name input

                match nameMatches with
                | true ->
                    let navigationResult =
                        State.get () |> Navigation.moveTo place.Id

                    match navigationResult with
                    | Ok effect ->
                        "You open the door to enter..." |> showMessage

                        wait 1000<millisecond>

                        effect |> Effect.apply
                    | Error PlaceEntranceError.CannotEnterOutsideOpeningHours ->
                        showSeparator None

                        World.placeClosedError place |> showMessage
                        World.placeOpeningHours place |> showMessage
                    | Error PlaceEntranceError.CannotEnterWithoutRental ->
                        showSeparator None

                        Styles.error
                            "You cannot enter this place without renting it first"
                        |> showMessage

                        Styles.information
                            "Try to use your phone to rent it out and come back again afterwards"
                        |> showMessage

                    Scene.WorldAfterMovement
                | false ->
                    $"There are no places called {input} around here"
                    |> Styles.error
                    |> showMessage

                    Scene.World }
