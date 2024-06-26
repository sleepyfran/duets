namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Interactions
open FsToolkit.ErrorHandling

/// Defines a command that can be executed by the user.
type Command =
    { Name: string
      Description: string
      Handler: string list -> Scene }

[<RequireQualifiedAccess>]
module Parse =
    /// <summary>
    /// Attempts to parse a phrase with the following structure:
    /// <c>[verb] [preposition] [item]</c> and returns the item. So that:
    /// <c>sleep on bed</c> returns bed.
    /// </summary>
    let itemAfterVerbWithPreposition prepositions (args: string seq) =
        match List.ofSeq args with
        | inputPreposition :: item when
            prepositions |> List.contains inputPreposition
            ->
            item |> String.concat " " |> Some
        | _ -> None

    /// Attempts to parse a phrase that contains any number of item names
    /// separated by the given separator.
    let rec itemsSeparatedBy (separator: string) (args: string list) =
        args |> String.concat " " |> String.splitS separator |> List.ofArray

[<RequireQualifiedAccess>]
module Selection =
    type SelectionResult<'a> =
        | Selected of 'a
        | Cancelled
        | NoMatchingItem of input: string

    let private interactiveSelection prompt items toString =
        showSearchableOptionalChoicePrompt prompt Generic.nothing toString items
        |> Option.map Selected
        |> Option.defaultValue Cancelled

    let private findFromArgs input items toString =
        let foundItem =
            items
            |> List.tryFind (fun item ->
                let itemName = toString item
                String.diacriticInsensitiveContains itemName input)

        match foundItem with
        | Some item -> Selected item
        | None -> NoMatchingItem input

    /// Attempts to parse an item from the given input or displays an interactive
    /// selector if no input is given. Returns an item if something matched
    /// or was selected, otherwise returns None or an error if the input did not
    /// match any item.
    let fromArgsOrInteractive args prompt items toDisplayName toReferenceName =
        let input = args |> String.concat " "

        if String.isEmpty input then
            interactiveSelection prompt items toDisplayName
        else
            findFromArgs input items toReferenceName

[<RequireQualifiedAccess>]
module Command =
    /// Creates a command with the given name and description that when called
    /// outputs the given message.
    let message name description message =
        { Name = name
          Description = description
          Handler =
            (fun _ ->
                showMessage message

                Scene.World) }

    /// Creates a command with the given name and description that when called
    /// calls the given handler.
    let create name description handler =
        { Name = name
          Description = description
          Handler = handler }

    /// Creates a command with the given name and description that when called
    /// executes the given action.
    let action name description action =
        create name description (fun _ ->
            action |> Effect.applyAction
            Scene.World)

    /// Creates a placeholder command with the given name that when called
    /// outputs the name of the command.
    let placeholder name = message name name name

    type ItemInteractionInput =
        | VerbOnly of verb: string
        | VerbWithPrepositions of verb: string * prepositions: string list

    let private verb input =
        match input with
        | VerbOnly verb -> verb
        | VerbWithPrepositions(verb, _) -> verb

    let private usageSample input =
        match input with
        | VerbOnly verb -> $"{verb} [[item]]"
        | VerbWithPrepositions(verb, prepositions) ->
            let formattedPrepositions =
                prepositions
                |> List.fold (fun acc preposition -> $"{preposition}|{acc}") ""

            $"{verb} {formattedPrepositions} [[item]]"

    /// Finds an item in either the world or the inventory by its name or
    /// alternative names.
    let findItem input =
        let input = String.trim input
        let currentPosition = Queries.World.currentCoordinates (State.get ())

        Queries.Items.allIn (State.get ()) currentPosition
        @ Queries.Inventory.character (State.get ())
        |> List.tryFind (fun item ->
            let itemAlternativeNames = Items.itemAlternativeNames item

            let brandMentioned =
                String.diacriticInsensitiveContains item.Brand input

            let nameMentioned =
                String.diacriticInsensitiveContains item.Name input

            let altNameMentioned =
                List.exists
                    (fun altName ->
                        String.diacriticInsensitiveContains altName input)
                    itemAlternativeNames

            brandMentioned || nameMentioned || altNameMentioned)

    /// <summary>
    /// Generates a command that can be invoked via the given verb with optional
    /// prepositions to perform a custom action on an item. Invokes <c>handler</c>
    /// with the name of the item to perform the action on and handles all errors
    /// to malformed input or not found items.
    /// </summary>
    let customItemInteraction input description handler =
        { Name = verb input
          Description = description
          Handler =
            fun args ->
                let itemName =
                    match input with
                    | VerbOnly _ -> args |> String.concat " " |> Some
                    | VerbWithPrepositions(_, prepositions) ->
                        Parse.itemAfterVerbWithPreposition prepositions args

                match itemName with
                | Some itemName when not (String.isEmpty itemName) ->
                    let item = findItem itemName

                    match item with
                    | Some item -> handler item
                    | None ->
                        Items.itemNotFound itemName |> showMessage
                        Scene.World
                | Some _
                | None ->
                    usageSample input |> Command.wrongUsage |> showMessage

                    Scene.World }

    /// <summary>
    /// Generates a command that can be invoked via the given verb with optional
    /// prepositions to perform an action on an item. Invokes <c>afterInteractionFn</c>
    /// with the result of the action performed and handles all errors related
    /// to malformed input or not found items.
    /// </summary>
    let itemInteraction input description interactionType afterInteractionFn =
        customItemInteraction input description (fun item ->
            Items.perform (State.get ()) item interactionType
            |> afterInteractionFn)

    /// Disables the command for a given reason, which removes the actual handler
    /// of the command and mocks it with a message displaying the reason why the
    /// action is not possible.
    let disable disabledReason command =
        { command with
            Handler =
                (fun _ ->
                    match disabledReason with
                    | InteractionDisabledReason.NotEnoughAttribute(attribute,
                                                                   amountNeeded) ->
                        match attribute with
                        | CharacterAttribute.Energy ->
                            Command.disabledNotEnoughEnergy amountNeeded
                        | CharacterAttribute.Health ->
                            Command.disabledNotEnoughHealth amountNeeded
                        | CharacterAttribute.Mood ->
                            Command.disabledNotEnoughMood amountNeeded
                        | _ -> ""
                    |> showMessage

                    Scene.World) }
