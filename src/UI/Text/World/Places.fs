module UI.Text.World.Places

open Avalonia.Controls.Documents
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Common
open Entities
open UI.Text.Base

let private descriptorText descriptor =
    match descriptor with
    | Beautiful -> "Beautiful"
    | Boring -> "Boring"
    | Central -> "Central"
    | Historical -> "Historical"

let private genericDescription typeName name descriptors =
    Span.create [
        Span.inlines [
            Bold.simple name :> IView
            Run.create [
                Run.text
                    $""" is a {listOf descriptors (descriptorText >> String.lowercase)} {typeName}"""
            ]
        ]
    ]

let private streetDescription =
    genericDescription "street"

let private boulevardDescription =
    genericDescription "boulevard"

let private squareDescription =
    genericDescription "square"

let outsideCoordinatesDescription coords =
    (coords.Node.Name, coords.Node.Descriptors)
    ||> match coords.Node.Type with
        | OutsideNodeType.Boulevard -> boulevardDescription
        | OutsideNodeType.Street -> streetDescription
        | OutsideNodeType.Square -> squareDescription
