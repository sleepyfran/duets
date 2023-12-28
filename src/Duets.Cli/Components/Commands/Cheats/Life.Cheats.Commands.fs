namespace Duets.Cli.Components.Commands.Cheats

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components.Commands
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module LifeCommands =
    /// Command which gives 100 to all needs of the character.
    let happy =
        { Name = "happy"
          Description = "Sets energy, health, hunger and mood to 100%"
          Handler =
            (fun _ ->
                let character =
                    Queries.Characters.playableCharacter (State.get ())

                [ CharacterAttribute.Energy
                  CharacterAttribute.Health
                  CharacterAttribute.Hunger
                  CharacterAttribute.Mood ]
                |> List.iter (fun attr ->
                    Character.Attribute.add character attr 100
                    |> Effect.applyMultiple)

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
