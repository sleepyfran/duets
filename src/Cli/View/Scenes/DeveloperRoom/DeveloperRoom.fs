module Cli.View.Scenes.DeveloperRoom

open Cli.View.TextConstants
open Cli.View.Actions
open Common
open Entities
open Simulation.Time.AdvanceTime
open Simulation.Queries

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
            yield
                Effect
                <| MoneyTransferred(characterAccount, Incoming 40000<dd>)

            yield SceneAfterKey DeveloperRoom
        | "iamrich" ->
            yield
                Effect
                <| MoneyTransferred(characterAccount, Outgoing 40000<dd>)

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
