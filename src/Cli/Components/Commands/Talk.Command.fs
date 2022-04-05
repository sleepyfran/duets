namespace Cli.Components.Commands

open FSharpx
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Entities

[<RequireQualifiedAccess>]
module TalkCommand =
    type TalkOption = Text * (unit -> Scene)

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
    /// - Option 1
    /// - Option 2
    /// - Option 3
    ///
    /// And once the player selects an option it'll execute the action chain
    /// passed for that option.
    let rec create npcs =
        { Name = "talk to"
          Description = I18n.translate (CommandText CommandTalkDescription)
          Handler =
              (fun args ->
                  match args with
                  | args when args.Length > 0 -> executeTalkCommand npcs args
                  | _ ->
                      I18n.translate (CommandText CommandTalkInvalidInput)
                      |> showMessage

                      Scene.World) }

    and private executeTalkCommand npcs args =
        let referencedName = String.concat " " args

        if referencedName = "" then
            npcNotFound referencedName
        else
            executeTalkCommand' npcs referencedName

    and private executeTalkCommand' npcs referencedName =
        let referencedNpc =
            npcs
            |> List.tryFind
                (fun talkingNpc ->
                    talkingNpc.Npc.Name = referencedName
                    || talkingNpc.Npc.Name
                       |> String.startsWith referencedName)

        match referencedNpc with
        | Some talkingNpc -> showNpcOptions talkingNpc
        | None -> npcNotFound referencedName

    and private npcNotFound referencedName =
        CommandTalkNpcNotFound referencedName
        |> CommandText
        |> I18n.translate
        |> showMessage

        Scene.World

    and private showNpcOptions talkingNpc =
        let selectedChoice =
            showOptionalChoicePrompt
                talkingNpc.Prompt
                (CommandText CommandTalkNothing |> I18n.translate)
                fst
                talkingNpc.Options

        match selectedChoice with
        | Some (_, handler) -> handler ()
        | None -> Scene.World
