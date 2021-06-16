module Cli.View.Scenes.DeveloperRoom

open Cli.View.Actions
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

    let characterMember =
        Bands.currentMemberById state character.Id
        |> Option.get

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
                [ Composition
                  Genre(band.Genre)
                  Instrument(characterMember.Role) ]
                |> Seq.map (Skills.characterSkillWithLevel state character.Id)
                |> Seq.map
                    (fun (skill, level) ->
                        (character, Diff((skill, level), (skill, 100))))
                |> Seq.map SkillImproved
                |> Seq.map Effect

            yield SceneAfterKey DeveloperRoom
        | "tick" ->
            yield
                advanceTimeOnce state.Today
                |> TimeAdvanced
                |> Effect
                
            yield Scene DeveloperRoom
        | _ -> yield Scene Map
    }
#endif
