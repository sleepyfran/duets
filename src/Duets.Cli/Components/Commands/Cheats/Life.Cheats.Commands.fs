namespace Duets.Cli.Components.Commands.Cheats

open System
open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.Components.Commands
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Time.AdvanceTime
open FSharpx

[<RequireQualifiedAccess>]
module LifeCommands =
    let private makeHappy () =
        let character = Queries.Characters.playableCharacter (State.get ())

        [ CharacterAttribute.Energy
          CharacterAttribute.Health
          CharacterAttribute.Hunger
          CharacterAttribute.Mood ]
        |> List.iter (fun attr ->
            Character.Attribute.add character attr 100 |> Effect.applyMultiple)

    /// Command which gives 100 to all needs of the character.
    let happy =
        { Name = "happy"
          Description = "Sets energy, health, hunger and mood to 100%"
          Handler =
            (fun _ ->
                makeHappy ()

                Scene.Cheats) }

    /// Command which removes all moodlets of the character.
    let notMoody =
        { Name = "not moody"
          Description = "Removes all your current moodlets"
          Handler =
            (fun _ ->
                let character =
                    Queries.Characters.playableCharacter (State.get ())

                let moodlets = Queries.Characters.moodlets character

                CharacterMoodletsChanged(
                    character.Id,
                    Diff(moodlets, Set.empty)
                )
                |> Effect.apply

                Scene.Cheats) }

    /// Command which lets the user advance in time.
    let timeTravel =
        { Name = "time travel"
          Description =
            "Advances the time by the number of day moments specified in the argument, keeping your character happy"
          Handler =
            (fun times ->
                "Travelling in time..." |> Styles.information |> showMessage

                let joinedArgs = String.Join("", times)

                match Int32.TryParse joinedArgs with
                | true, n ->
                    [ 1..n ]
                    |> List.iter (fun _ ->
                        makeHappy ()

                        advanceDayMoment' (State.get ()) 1<dayMoments>
                        |> Effect.applyMultiple)

                    let currentTime = Queries.Calendar.today (State.get ())

                    $"You have travelled in time and it's now {Styles.time currentTime}"
                    |> showMessage

                    Scene.Cheats
                | false, _ -> Scene.Cheats) }
