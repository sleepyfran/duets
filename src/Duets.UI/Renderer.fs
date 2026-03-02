module Duets.UI.Renderer

open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Layout
open Avalonia.Media
open Duets.Entities
open Duets.UI.Common
open Duets.UI.Theme
open System.Threading.Tasks

// ── Widget builders ───────────────────────────────────────────────────────────

let private choiceWidget
    (values: 'T list)
    (display: 'T -> string)
    (onSelected: 'T -> unit)
    : IView =
    StackPanel.create
        [ StackPanel.orientation Orientation.Vertical
          StackPanel.spacing Padding.small
          StackPanel.children
              [ for v in values do
                    Button.create
                        [ Button.content (display v)
                          Button.onClick (fun _ -> onSelected v)
                          Button.classes [ "menu" ] ]
                    :> IView ] ]
    :> IView

let private textBoxWidget
    (placeholder: string)
    (parse: string -> 'T option)
    (onConfirmed: 'T -> unit)
    : IView =
    let inputState = new State<string>("")

    Component.create (
        $"TextAsk-{placeholder}",
        fun ctx ->
            let text = ctx.usePassed inputState

            StackPanel.create
                [ StackPanel.spacing Padding.small
                  StackPanel.children
                      [ TextBox.create
                            [ TextBox.watermark placeholder
                              TextBox.onTextChanged text.Set ]
                        Button.create
                            [ Button.content "Confirm"
                              Button.isEnabled (
                                  parse text.Current |> Option.isSome
                              )
                              Button.onClick (fun _ ->
                                  parse text.Current |> Option.iter onConfirmed) ] ] ]
    )
    :> IView

let private confirmWidget
    (question: string)
    (onConfirmed: bool -> unit)
    : IView =
    StackPanel.create
        [ StackPanel.spacing Padding.small
          StackPanel.children
              [ TextBlock.create [ TextBlock.text question ]
                StackPanel.create
                    [ StackPanel.orientation Orientation.Horizontal
                      StackPanel.spacing Padding.small
                      StackPanel.children
                          [ Button.create
                                [ Button.content "Yes"
                                  Button.onClick (fun _ -> onConfirmed true) ]
                            :> IView
                            Button.create
                                [ Button.content "No"
                                  Button.classes [ "destructive" ]
                                  Button.onClick (fun _ -> onConfirmed false) ]
                            :> IView ] ]
                :> IView ] ]
    :> IView

// ── Show content renderer ─────────────────────────────────────────────────────

let private renderShow (content: ShowContent) : IView =
    match content with
    | ShowContent.Text msg ->
        TextBlock.create
            [ TextBlock.text msg; TextBlock.textWrapping TextWrapping.Wrap ]
        :> IView
    | ShowContent.Figlet text ->
        TextBlock.create
            [ TextBlock.text text
              TextBlock.fontSize 64
              TextBlock.fontWeight FontWeight.Bold ]
        :> IView
    | ShowContent.GameInfo version ->
        TextBlock.create
            [ TextBlock.text $"v{version}"
              TextBlock.fontSize 20
              TextBlock.foreground Brush.bg
              TextBlock.horizontalAlignment HorizontalAlignment.Right ]
        :> IView
    | ShowContent.Separator label ->
        StackPanel.create
            [ StackPanel.spacing Padding.small
              StackPanel.children
                  [ match label with
                    | Some text ->
                        TextBlock.create
                            [ TextBlock.text text
                              TextBlock.foreground Brush.bg ]
                        :> IView
                    | None -> ()
                    Duets.UI.Components.Divider.vertical ] ]
        :> IView
    | ShowContent.LineBreak ->
        Border.create [ Border.height (float Padding.medium) ] :> IView
    | _ ->
        // Stub: content types not yet needed for MainMenu/NewGame
        Border.create [] :> IView

// ── Renderer factory ──────────────────────────────────────────────────────────

/// Creates a FuncUI IRenderer that accumulates views into the given writable
/// state list. Ask operations render interactive widgets and complete when the
/// user acts.
let create (viewStack: IWritable<IView list>) : IRenderer =
    let add (view: IView) =
        viewStack.Set(viewStack.Current @ [ view ])

    { new IRenderer with
        member _.Show content = async { add (renderShow content) }

        member _.Ask(content: AskContent<'T>) =
            let tcs = TaskCompletionSource<'T>()

            let widget =
                match content with
                | AskContent.Choice(values, display) ->
                    choiceWidget values display (fun v ->
                        tcs.TrySetResult v |> ignore)
                | AskContent.SearchChoice(values, display) ->
                    choiceWidget values display (fun v ->
                        tcs.TrySetResult v |> ignore)
                | AskContent.TextBox(placeholder, parse) ->
                    textBoxWidget placeholder parse (fun v ->
                        tcs.TrySetResult v |> ignore)

            add widget
            tcs.Task |> Async.AwaitTask

        member _.AskMultiple(values, display) =
            // Stub: not needed for MainMenu/NewGame
            async { return [] }

        member _.Confirm question =
            let tcs = TaskCompletionSource<bool>()
            add (confirmWidget question (fun b -> tcs.TrySetResult b |> ignore))
            tcs.Task |> Async.AwaitTask

        member _.Continue() =
            let tcs = TaskCompletionSource<unit>()

            let widget =
                Button.create
                    [ Button.content "Continue"
                      Button.onClick (fun _ -> tcs.TrySetResult() |> ignore) ]
                :> IView

            add widget
            tcs.Task |> Async.AwaitTask

        member _.AskCity _ =
            // Stub: not needed for MainMenu/NewGame
            async { return None }

        member _.AskDate(_, _) =
            // Stub: not needed for MainMenu/NewGame
            async { return None }

        member _.AskCommand(_, _) =
            // Stub: InGame is not yet migrated
            async { return obj () } }

/// Wraps a Scene<unit> in a FuncUI Component that manages its own view stack
/// and runs the scene on mount.
let run (key: string) (sceneDesc: Scene<unit>) : IView =
    Component.create (
        $"SceneRunner-{key}",
        fun ctx ->
            let views = ctx.useState []

            ctx.useEffect (
                handler =
                    (fun _ ->
                        let renderer = create views
                        async { do! sceneDesc renderer } |> Async.Start),
                triggers = [ EffectTrigger.AfterInit ]
            )

            StackPanel.create
                [ StackPanel.horizontalAlignment HorizontalAlignment.Center
                  StackPanel.spacing Padding.medium
                  StackPanel.children [ yield! views.Current ] ]
    )
    :> IView
