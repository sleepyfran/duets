module Cli.View.Scenes.DeveloperRoom

open Cli.View.Actions
open Entities
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
        | _ -> yield Scene Map
    }
#endif