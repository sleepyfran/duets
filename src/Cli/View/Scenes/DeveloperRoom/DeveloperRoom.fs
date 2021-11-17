module Cli.View.Scenes.DeveloperRoom

open Cli.View.TextConstants
open Cli.View.Actions
open Common
open Entities
open Simulation.Queries
open Simulation.Bank.Operations
open Simulation.Time.AdvanceTime

#if DEBUG
/// Tools for when developing. Allows commands that can make debugging easier.
let rec developerRoom state =
    seq {
        yield
            Prompt
                { Title = Literal "[bold blue]duets_dev>[/]"
                  Content = TextPrompt <| processCommand state }
    }

and processCommand state command =
    let character = Characters.playableCharacter state
    let band = Bands.currentBand state
    let characterAccount = Character character.Id

    seq {
        match command with
        | "motherlode" ->
            yield Effect <| income state characterAccount 40000<dd>

            yield SceneAfterKey DeveloperRoom
        | "iamrich" ->
            yield!
                expense state characterAccount 40000<dd>
                |> fun res ->
                    match res with
                    | Ok effects -> effects |> List.map Effect
                    | Error _ ->
                        [ Message
                          <| Literal "May not as much as you think..." ]

            yield SceneAfterKey DeveloperRoom
        | "madskillz" ->
            yield!
                band.Members
                |> Seq.map
                    (fun currentMember ->
                        [ Composition
                          Genre(band.Genre)
                          SkillId.Instrument(currentMember.Role) ]
                        |> Seq.map (
                            Skills.characterSkillWithLevel
                                state
                                currentMember.Character.Id
                        )
                        |> Seq.map
                            (fun (skill, level) ->
                                (currentMember.Character,
                                 Diff((skill, level), (skill, 100))))
                        |> Seq.map SkillImproved
                        |> Seq.map Effect)
                |> Seq.concat

            yield SceneAfterKey DeveloperRoom
        | command when command.StartsWith "tick" ->
            let times =
                match parseParams command with
                | [| times |] -> int times
                | _ -> 1

            yield!
                advanceDayMoment state.Today times
                |> Seq.map Effect

            yield Scene DeveloperRoom
        | _ -> yield Scene Map
    }

and parseParams (command: string) = command.Split ' ' |> Array.skip 1
#endif
