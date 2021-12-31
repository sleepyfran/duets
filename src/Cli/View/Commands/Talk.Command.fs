namespace Cli.View.Commands

open FSharpx
open Cli.View.Actions
open Cli.View.Common
open Cli.View.Text
open Entities

[<RequireQualifiedAccess>]
module TalkCommand =
    type TalkOption = Text * ActionChain

    type TalkingNpc =
        { Npc: Character
          Prompt: Text
          Options: TalkOption list }

    /// Creates a command that allows the player to talk to NPCs using the
    /// `talk to {name}`. Accepts a list of NPCs that are around the player
    /// and the set of options to show the player when they execute the command
    /// on an NPC.
    ///
    /// It'll always show as:
    /// > Blablabla?
    /// - Blabla
    /// - Bla
    /// - B
    ///
    /// And once the player selects an option it'll execute the action chain
    /// passed for that option.
    let rec create npcs =
        { Name = "talk"
          Description = I18n.translate (CommandText CommandTalkDescription)
          Handler =
              HandlerWithNavigation
                  (fun args ->
                      match args with
                      | toKeyword :: name when toKeyword = "to" ->
                          executeTalkCommand npcs name
                      | _ ->
                          seq {
                              I18n.translate (
                                  CommandText CommandTalkInvalidInput
                              )
                              |> Message

                              Scene Scene.World
                          }) }

    and private executeTalkCommand npcs name =
        let referencedName = String.concat " " name

        let referencedNpc =
            npcs
            |> List.tryFind
                (fun talkingNpc ->
                    talkingNpc.Npc.Name = referencedName
                    || talkingNpc.Npc.Name
                       |> String.contains referencedName)

        match referencedNpc with
        | Some talkingNpc -> showNpcOptions talkingNpc
        | None ->
            seq {
                CommandTalkNpcNotFound referencedName
                |> CommandText
                |> I18n.translate
                |> Message

                Scene Scene.World
            }

    and private showNpcOptions talkingNpc =
        let choicesWithId =
            talkingNpc.Options
            |> List.map (fun (text, chain) -> (Identity.create (), text, chain))

        let choices =
            choicesWithId
            |> List.map
                (fun (id, text, _) -> { Id = id.ToString(); Text = text })

        seq {
            Prompt
                { Title = talkingNpc.Prompt
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = choices
                            Handler =
                                worldOptionalChoiceHandler (
                                    handleChoiceSelection choicesWithId
                                )
                            BackText =
                                I18n.translate (CommandText CommandTalkNothing) } }
        }

    and private handleChoiceSelection choicesWithId choice =
        choicesWithId
        |> List.find (fun (id, _, _) -> id.ToString() = choice.Id)
        |> (fun (_, _, chain) -> chain)
