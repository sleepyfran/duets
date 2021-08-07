[<AutoOpen>]
module Duets.Scenes.Base.Table

open Nez.UI

open Duets.Scenes
open Duets.Text.English
open Nez.UI

type TextSize =
    | Body
    | Title

/// Function to be passed as a styleFn to any Add* method that applies no style
/// to the given cell.
let noStyle = fun _ -> ()

/// Function to be passed as a styleFn to any Add* method that applies a
/// centered style to the given cell.
let centered = fun (cell: Cell) -> cell.Center()

let createButton text handler =
    let btn = TextButton(toString text, skin)
    btn.add_OnClicked (fun _ -> handler ())
    btn

/// Wraps a Nez table adding utility method to add content into the table.
type Table() =
    inherit Nez.UI.Table()

    member private this.NewRow() = this.Row() |> ignore

    /// Adds a new label given its text and size and applies the given styleFn
    /// to the wrapping cell.
    member this.AddText text size styleFn =
        let fontScale =
            match size with
            | Body -> 1.0f
            | Title -> 10.0f

        let label =
            this.Add(Label(toString text).SetFontScale(fontScale))

        styleFn label |> ignore

        this.NewRow()

    /// Same as AddText but applies the default Body size to the text.
    member this.AddBodyText text styleFn = this.AddText text Body styleFn

    /// Adds a button to the root view and applies the given styleFn to the
    /// wrapping cell.
    member this.AddButton text handler styleFn =
        createButton text handler
        |> this.Add
        |> styleFn
        |> ignore

        this.NewRow()

    /// Adds a new text input to the table with the specified label text and
    /// a handler that will receive each update of the field. The styleFn will
    /// be applied to the container of both the label and the input.
    member this.AddInput labelText handler styleFn =
        let inputTable = Table()
        this.Add(inputTable) |> styleFn |> ignore

        inputTable
            .Add(Label(toString labelText, skin))
            .Left()
        |> ignore

        inputTable.NewRow()

        inputTable
            .Add(TextField("", skin))
            .Left()
            .GetElement<TextField>()
            .add_OnTextChanged (fun _ text -> handler text)

        this.NewRow()

    /// Adds a selector of the given elements prepending a label and calling
    /// the specified handler each time a new element is selected.
    member this.AddSelector<'a>
        labelText
        (elements: seq<'a>)
        (handler: 'a -> unit)
        (styleFn: Cell -> Cell)
        =
        let selectorTable = Table()
        this.Add(selectorTable) |> styleFn |> ignore

        selectorTable
            .Add(Label(toString labelText, skin))
            .Left()
        |> ignore

        selectorTable.NewRow()

        let selectBoxCell = selectorTable.Add(SelectBox<'a>(skin))
        styleFn selectBoxCell |> ignore

        let selectBoxElement =
            selectBoxCell.GetElement<SelectBox<'a>>()

        selectBoxElement.SetItems(ResizeArray(elements))
        selectBoxElement.OnChanged <- System.Action<'a>(handler)

        this.NewRow()
